using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MemoryAPI;
using MemoryAPI.Navigation;


public class NavMesh
{

	static int NAVMESHSET_MAGIC = 'M' << 24 | 'S' << 16 | 'E' << 8 | 'T'; //'MSET';
	static int NAVMESHSET_VERSION = 1;
	private Detour.dtNavMesh dtNavMesh;
	private Zone _zone;

	public NavMesh()
	{
	}

	public struct NavMeshSetHeader
	{
		public int magic;
		public int version;
		public int numTiles;
		public Detour.dtNavMeshParams meshParams;

		public static int ByteSize()
		{
			var start = IndexOfTileHeaderBytes();

			start += Detour.dtNavMeshParams.ByteSize();

			return start;
		}

		public static int IndexOfTileHeaderBytes()
		{
			var start = 0;

			start += sizeof(int);
			start += sizeof(int);
			start += sizeof(int);

			return start;
		}

		public int FromBytes(byte[] array, int start)
		{
			magic = BitConverter.ToInt32(array, start); start += sizeof(int);
			version = BitConverter.ToInt32(array, start); start += sizeof(int);
			numTiles = BitConverter.ToInt32(array, start); start += sizeof(int);
			meshParams = new Detour.dtNavMeshParams();
			meshParams.FromBytes(array, start); start += Detour.dtNavMeshParams.ByteSize();

			return start;
		}
	}
	public struct NavMeshTileHeader
	{
		public uint tileRef;
		public int dataSize;

		public static int ByteSize()
		{
			var start = 0;

			start += sizeof(uint);
			start += sizeof(int);

			return start;
		}

		public int FromBytes(byte[] array, int start)
		{
			tileRef = BitConverter.ToUInt32(array, start); start += sizeof(uint);
			dataSize = BitConverter.ToInt32(array, start); start += sizeof(int);

			return start;
		}
	}

	public bool LoadZone(Zone zone)

	{
		if (zone == Zone.Unknown)
		{
			Unload();
			return false;
		}

		string path = string.Format(@"NavigationMeshes\{0}.nav", zone.ToString());

		if (_zone == zone && dtNavMesh != null)
		{
			return true;
		}
		else
		{
			Unload();
		}

		var headerBufferSize = NavMeshSetHeader.ByteSize();
		var headerBuffer = new byte[headerBufferSize];

		if (!File.Exists(path))
		{
			return false;
		}

		var file = File.OpenRead(path);
		file.Read(headerBuffer, 0, headerBufferSize);

		NavMeshSetHeader header = new NavMeshSetHeader();
		var headerBytesRead = header.FromBytes(headerBuffer, 0);

		if (header.magic != NAVMESHSET_MAGIC)
		{
			return false;
		}

		if (header.version != NAVMESHSET_VERSION)
		{
			return false;
		}

		var navMesh = new Detour.dtNavMesh();
		navMesh.init(header.meshParams);

		for (int i = 0; i < header.numTiles; ++i)
		{
			var tileHeaderBuffer = new byte[NavMeshTileHeader.ByteSize()];
			file.Read(tileHeaderBuffer, 0, tileHeaderBuffer.Length);
			var tileHeader = new NavMeshTileHeader();
			tileHeader.FromBytes(tileHeaderBuffer, 0);
			if (tileHeader.dataSize == 0 || tileHeader.tileRef == 0)
			{
				break;
			}
			var rawTileData = new Detour.dtRawTileData();
			var data = new byte[tileHeader.dataSize];
			file.Read(data, 0, data.Length);
			rawTileData.FromBytes(data, 0);
			uint result = 0;
			navMesh.addTile(rawTileData, tileHeader.dataSize, 0x01 /*DT_TILE_FREE_DATA*/, tileHeader.tileRef, ref result);
			if (Detour.dtStatusFailed(result))
			{
				return false;
			}
		}

		// hard-code to make sure it is compatible with expectation.
		var maxPolys = header.meshParams.maxPolys;

		var status = new Detour.dtNavMeshQuery().init(navMesh, maxPolys);

		if (Detour.dtStatusFailed(status))
		{
			return false;
		}

		dtNavMesh = navMesh;

		return headerBytesRead > 0;
	}

	public void Unload()
	{
		dtNavMesh = null;
	}

	private static Position ToFFXIPosition(float[] detourPosition)
	{
		var ffxiPosition = new Position();
		
		ffxiPosition.X = detourPosition[0];
		ffxiPosition.Y = -detourPosition[1];
		ffxiPosition.Z = -detourPosition[2];

		return ffxiPosition;

	}

	public Queue<Position> FindPathBetween(Position start, Position end, bool useStraightPath = false)
	{
		var path = new Queue<Position>();

		if (dtNavMesh == null)
		{
			return path;
		}
		var startDetourPosition = start.ToDetourPosition();
		var endDetourPosition = end.ToDetourPosition();

		var queryFilter = new Detour.dtQueryFilter();
		var navMeshQuery = new Detour.dtNavMeshQuery();

		var status = navMeshQuery.init(dtNavMesh, 256);

		if (Detour.dtStatusFailed(status))
		{
			return path;
		}

		queryFilter.setIncludeFlags(0xffff);
		queryFilter.setExcludeFlags(0x0);

		uint startRef = 0;
		uint endRef = 0;
		float[] startNearest = new float[3];
		float[] endNearest = new float[3];

		float[] extents = new float[] { 10.0F, (float)EasyFarm.UserSettings.Config.Instance.HeightThreshold, 10.0F };

		status = navMeshQuery.findNearestPoly(startDetourPosition, extents, queryFilter, ref startRef, ref startNearest);

		if (Detour.dtStatusFailed(status))
		{
			return path;
		}

		status = navMeshQuery.findNearestPoly(endDetourPosition, extents, queryFilter, ref endRef, ref endNearest);

		if (Detour.dtStatusFailed(status))
		{
			return path;
		}

		if (!dtNavMesh.isValidPolyRef(startRef) || !dtNavMesh.isValidPolyRef(endRef))
		{
			return path;
		}

		uint[] pathPolys = new uint[256];

		int pathCount = 0;

		status = navMeshQuery.findPath(startRef, endRef, startNearest, endNearest, queryFilter, pathPolys, ref pathCount, 256);

		if (Detour.dtStatusFailed(status))
		{
			return path;
		}

		if (path.Count < 1)
        {
            float[] straightPath = new float[256 * 3];
            byte[] straightPathFlags = new byte[256];
            uint[] straightPathPolys = new uint[256];
            int straightPathCount = 256 * 3;

            status = navMeshQuery.findStraightPath(
                startNearest,
                endNearest,
                pathPolys,
                pathCount,
                straightPath,
                straightPathFlags,
                straightPathPolys,
                ref straightPathCount,
                256,
                0
            );

            if (straightPathCount > 1)
            {
                if (Detour.dtStatusFailed(status))
                {
                    return path;
				}

				path.Clear();

				// i starts at 3 so the start position is ignored
				for (int i = 3; i < straightPathCount * 3;)
                {
                    float[] pathPos = new float[3];
                    pathPos[0] = straightPath[i++];
                    pathPos[1] = straightPath[i++];
                    pathPos[2] = straightPath[i++];

                    var position = ToFFXIPosition(pathPos);

                    path.Enqueue(position);
                }
            }
        } 
		else
		{
			// i starts at 3 so the start position is ignored
			for (int i = 1; i < pathCount; i++)
			{
				float[] pathPos = new float[3];
				bool posOverPoly = false;
				if (Detour.dtStatusFailed(navMeshQuery.closestPointOnPoly(pathPolys[i], startDetourPosition, pathPos, ref posOverPoly)))
					return path;

				if (path.Count < 1)
				{
					if (Detour.dtStatusFailed(navMeshQuery.closestPointOnPolyBoundary(pathPolys[i], startDetourPosition, pathPos)))
						return path;
				}

				var position = ToFFXIPosition(pathPos);

				path.Enqueue(position);
			}
		}

        return path;

	}

	public Position NextRandomPosition(Position start)
	{
		return new Position();
	}
}
