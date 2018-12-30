using System;
using System.Collections.Generic;
using System.Linq;
using EasyFarm.Classes;
using EasyFarm.States;
using EasyFarm.UserSettings;
using MemoryAPI;

namespace EasyFarm.Context
{
    public class GameContext : IGameContext
    {
        public GameContext(IMemoryAPI api)
        {
            API = api;
            Player = new Player(api);
            Config = new ProxyConfig();
            Memory = new StateMemory(api);
        }

        public IConfig Config { get; set; }
        public IPlayer Player { get; set; }
        public IUnit Target { get; set; }
        public Boolean IsFighting { get; set; }
        public Zone Zone { get;set; }

        public IList<IUnit> Units
        {
            get => Memory.UnitService.MobArray.ToList();
            set => throw new NotImplementedException();
        }

        /*
             Allow code using old API to continue using it until we move things over. 
             We'll need to figure out how we can make services like Executor.UseActions testable.
        */

        [Obsolete("Please consider using GameContext instead.")]
        public IMemoryAPI API { get; set; }
        [Obsolete("Please consider using GameContext instead.")]
        public StateMemory Memory { get; set; }
    }
}