// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013 Mykezero
//  
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
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
            Target = new NullUnit();
            MockAPI = new MockEliteAPI();
            API = new MockEliteAPIAdapter(MockAPI);
            Memory = new StateMemory(API);
        }

        public void SetPlayerInjured()
        {
            Config.IsHealthEnabled = true;
            Config.HighHealth = 50;
            Config.LowHealth = 0;
            MockAPI.Player.HPPCurrent = 25;
        }

        public void SetInvalidTarget()
        {
            Target.IsValid = false;
        }

        public void SetPlayerHealthy()
        {
            Config.IsHealthEnabled = false;
            Config.HighHealth = 50;
            Config.LowHealth = 0;
            MockAPI.Player.HPPCurrent = 75;
        }
    }
}