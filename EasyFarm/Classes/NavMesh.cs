using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using MemoryAPI;
using MemoryAPI.Navigation;


public class NavMesh
{
	private Detour.dtNavMesh dtNavMesh;
	private IMemoryAPI api;

	public NavMesh(IMemoryAPI api)
	{
		this.api = api;
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

			start += sizeof(int);
			start += sizeof(uint);

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

		string path = "navmeshes\\" + zone.ToString() + ".nav";
		if (this.dtNavMesh != null)
		{
			Unload();
		}

		//var context = new Recast.rcContext();
		//polyMesh = new Recast.rcPolyMesh();
		//Recast.rcBuildPolyMesh(context, )

		var headerBufferSize = NavMeshSetHeader.ByteSize();
		var headerBuffer = new byte[headerBufferSize];

		var file = File.OpenRead(path);
		file.Read(headerBuffer, 0, headerBufferSize);

		NavMeshSetHeader header = new NavMeshSetHeader();
		var headerBytesRead = header.FromBytes(headerBuffer, 0);

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
			//try
			//{
				rawTileData.FromBytes(data, 0);
			//} catch(Exception e)
			//{
			//	break;
			//}
			uint result = 0;
			navMesh.addTile(rawTileData, 0x01 /*DT_TILE_FREE_DATA*/, tileHeader.tileRef, ref result);
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
			//var meshHeader = new Detour.dtMeshHeader();

		//var meshHeaderResult = meshHeader.FromBytes(headerBuffer, 0);

		//var navMesh = new Detour.dtNavMesh();


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
		ffxiPosition.Z = -detourPosition[1];
		ffxiPosition.Y = -detourPosition[2];

		return ffxiPosition;

	}
	private static float[] ToDetourPosition(Position position)
	{
		float[] detourPosition = new float[3];
		
		detourPosition[0] = position.X;
		detourPosition[1] = -position.Z;
		detourPosition[2] = -position.Y;

		return detourPosition;
	}

	public Queue<Position> FindPathBetween(Position start, Position end)
	{
		if (dtNavMesh == null)
		{
			return new Queue<Position>(0);
		}

		var startDetourPosition = ToDetourPosition(start);
		var endDetourPosition = ToDetourPosition(end);

		var queryFilter = new Detour.dtQueryFilter();
		var navMeshQuery = new Detour.dtNavMeshQuery();

		var status = navMeshQuery.init(dtNavMesh, 256);

		if (Detour.dtStatusFailed(status))
		{
			return new Queue<Position>(0);
		}

		queryFilter.setIncludeFlags(0xffff);
		queryFilter.setIncludeFlags(0x0);

		uint startRef = 0;
		uint endRef = 0;
		float[] startNearest = new float[3];
		float[] endNearest = new float[3];

		float[] extents = new float[] { 10000.0F, 20000.0F, 10000.0F };

		status = navMeshQuery.findNearestPoly(startDetourPosition, extents, queryFilter, ref startRef, ref startNearest);

		if (Detour.dtStatusFailed(status))
		{
			return new Queue<Position>(0);
		}

		status = navMeshQuery.findNearestPoly(endDetourPosition, extents, queryFilter, ref endRef, ref endNearest);

		if (Detour.dtStatusFailed(status))
		{
			return new Queue<Position>(0);
		}

		if (!dtNavMesh.isValidPolyRef(startRef) || !dtNavMesh.isValidPolyRef(endRef))
		{
			return new Queue<Position>(0);
		}

		return new Queue<Position>();
	}

	public Position NextRandomPosition(Position start)
	{
		return new Position();
	}
}
