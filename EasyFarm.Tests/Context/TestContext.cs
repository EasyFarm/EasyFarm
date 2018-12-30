using System.Collections.Generic;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.States;
using EasyFarm.Tests.TestTypes.Mocks;
using EasyFarm.UserSettings;
using MemoryAPI;

namespace EasyFarm.Tests.Context
{
    public class TestContext : IGameContext
    {
        public IConfig Config { get; set; }
        public IPlayer Player { get; set; }
        public IUnit Target { get; set; }
        public bool IsFighting { get; set; }
        public Zone Zone { get; set; }
        public IList<IUnit> Units { get; set; }
        public IMemoryAPI API { get; set; }
        public StateMemory Memory { get; set; }

        public MockEliteAPI MockAPI { get; set; }

        public TestContext()
        {
            Units = new List<IUnit> { new MockUnit() };
            Config = new MockConfig();
            Player = new MockPlayer();
            Target = new MockUnit();
            MockAPI = new MockEliteAPI();
            API = new MockEliteAPIAdapter(MockAPI);
            Memory = new StateMemory(API);
        }
    }
}