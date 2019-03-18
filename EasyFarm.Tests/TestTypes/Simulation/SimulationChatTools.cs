using System.Collections.Generic;
using EliteMMO.API;
using MemoryAPI.Chat;

namespace EasyFarm.Tests.TestTypes.Simulation
{
    public class SimulationChatTools : IChatTools
    {
        private readonly ISimulation _simulation;

        public SimulationChatTools(ISimulation simulation)
        {
            _simulation = simulation;
        }

        public Queue<EliteAPI.ChatEntry> ChatEntries { get; set; } = new Queue<EliteAPI.ChatEntry>();
    }
}