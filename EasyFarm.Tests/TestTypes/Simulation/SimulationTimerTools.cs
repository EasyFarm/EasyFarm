using System;
using MemoryAPI;

namespace EasyFarm.Tests.TestTypes.Simulation
{
    public class SimulationTimerTools : ITimerTools
    {
        private readonly ISimulation _simulation;

        public SimulationTimerTools(ISimulation simulation)
        {
            _simulation = simulation;
        }

        public int GetAbilityRecast(int index)
        {
            throw new NotImplementedException();
        }

        public int GetSpellRecast(int index)
        {
            throw new NotImplementedException();
        }
    }
}