using System;
using EliteMMO.API;
using MemoryAPI.Windower;

namespace EasyFarm.Tests.TestTypes.Simulation
{
    public class SimulationWindowerTools : IWindowerTools
    {
        private readonly ISimulation _simulation;

        public SimulationWindowerTools(ISimulation simulation)
        {
            _simulation = simulation;
        }

        public void SendString(string stringToSend)
        {
            throw new NotImplementedException();
        }

        public void SendKeyPress(Keys key)
        {
            throw new NotImplementedException();
        }
    }
}