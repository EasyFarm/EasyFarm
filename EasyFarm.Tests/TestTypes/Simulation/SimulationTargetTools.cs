using System;
using MemoryAPI;

namespace EasyFarm.Tests.TestTypes.Simulation
{
    public class SimulationTargetTools : ITargetTools
    {
        private readonly ISimulation _simulation;

        public SimulationTargetTools(ISimulation simulation)
        {
            _simulation = simulation;
        }

        public int ID { get; }
        public bool SetNPCTarget(int index)
        {
            throw new NotImplementedException();
        }
    }
}