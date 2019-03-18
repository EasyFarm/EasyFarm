using System;
using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.Tests.TestTypes.Simulation
{
    public class SimulationNPC : INPCTools
    {
        private readonly ISimulation _simulation;

        public SimulationNPC(ISimulation simulation)
        {
            _simulation = simulation;
        }

        public int ClaimedID(int id)
        {
            return 0;
        }

        public double Distance(int id)
        {
            return 0;
        }

        public Position GetPosition(int id)
        {
            return new Position();
        }

        public short HPPCurrent(int id)
        {
            return 0;
        }

        public bool IsActive(int id)
        {
            return false;
        }

        public bool IsClaimed(int id)
        {
            return false;
        }

        public bool IsRendered(int id)
        {
            return false;
        }

        public string Name(int id)
        {
            return "";
        }

        public NpcType NPCType(int id)
        {
            return NpcType.InanimateObject;
        }

        public float PosX(int id)
        {
            return 0;
        }

        public float PosY(int id)
        {
            return 0;
        }

        public float PosZ(int id)
        {
            return 0;
        }

        public Status Status(int id)
        {
            return MemoryAPI.Status.Unknown;
        }

        public int PetID(int id)
        {
            return 0;
        }
    }
}