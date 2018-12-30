using System;
using System.Collections.Generic;
using EasyFarm.Classes;
using EasyFarm.States;
using EasyFarm.UserSettings;
using MemoryAPI;

namespace EasyFarm.Context
{
    public interface IGameContext
    {
        IConfig Config { get; set; }
        IPlayer Player { get; set; }
        IUnit Target { get; set; }
        Boolean IsFighting { get; set; }
        Zone Zone { get; set; }
        IList<IUnit> Units { get; set; }
        IMemoryAPI API { get; set; }
        StateMemory Memory { get; set; }
    }
}