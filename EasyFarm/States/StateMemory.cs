// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
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
using EasyFarm.Classes;
using EasyFarm.UserSettings;
using MemoryAPI;

namespace EasyFarm.States
{
    [Obsolete("Please consider using GameContext instead.")]
    public class StateMemory
    {
        public StateMemory(IMemoryAPI eliteApi)
        {
            EliteApi = eliteApi;
            Executor = new Executor(eliteApi);
            UnitService = new UnitService(eliteApi);
            UnitFilters = new UnitFilters();
            Config = new ProxyConfig();
        }

        public IConfig Config { get; set; }
        public IMemoryAPI EliteApi { get; set; }        
        public Executor Executor { get; set; }
        public IUnitService UnitService { get; set; }    
        public IUnitFilters UnitFilters { get; set; }

        public bool IsFighting { get; set; }
        public IUnit Target { get; set; }
        public Zone Zone { get; set; }
    }
}