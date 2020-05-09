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
        NavMesh NavMesh { get; }
        IList<IUnit> Units { get; set; }
        IMemoryAPI API { get; set; }
        StateMemory Memory { get; set; }
    }
}