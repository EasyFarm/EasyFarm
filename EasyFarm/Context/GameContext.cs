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
            Target = new NullUnit();
            NavMesh = new NavMesh();
            Zone = api.Player.Zone;
            NavMesh.LoadZone(_zone);
        }

        public IConfig Config { get; set; }
        public IPlayer Player { get; set; }
        public IUnit Target { get; set; }
        public Boolean IsFighting { get; set; }
        private Zone _zone;
        public Zone Zone
        {
            get
            {
                return _zone;
            }
            set
            {
                NavMesh.LoadZone(_zone);
                _zone = value;
            }
        }
        public NavMesh NavMesh { get; set; }

        public IList<IUnit> Units
        {
            get => Memory.UnitService.MobArray.ToList();
            set => throw new NotImplementedException();
        }

        /*
             Allow code using old API to continue using it until we move things over. 
             We'll need to figure out how we can make services like Executor.UseActions testable.
        */
        public IMemoryAPI API { get; set; }
        public StateMemory Memory { get; set; }
    }
}