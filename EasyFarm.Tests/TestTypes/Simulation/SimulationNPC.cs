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
            throw new NotImplementedException();
        }

        public double Distance(int id)
        {
            throw new NotImplementedException();
        }

        public Position GetPosition(int id)
        {
            throw new NotImplementedException();
        }

        public short HPPCurrent(int id)
        {
            throw new NotImplementedException();
        }

        public bool IsActive(int id)
        {
            throw new NotImplementedException();
        }

        public bool IsClaimed(int id)
        {
            throw new NotImplementedException();
        }

        public bool IsRendered(int id)
        {
            throw new NotImplementedException();
        }

        public string Name(int id)
        {
            throw new NotImplementedException();
        }

        public NpcType NPCType(int id)
        {
            throw new NotImplementedException();
        }

        public float PosX(int id)
        {
            throw new NotImplementedException();
        }

        public float PosY(int id)
        {
            throw new NotImplementedException();
        }

        public float PosZ(int id)
        {
            throw new NotImplementedException();
        }

        public Status Status(int id)
        {
            throw new NotImplementedException();
        }

        public int PetID(int id)
        {
            throw new NotImplementedException();
        }
    }
}