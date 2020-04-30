﻿// ///////////////////////////////////////////////////////////////////
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
using EasyFarm.Context;
using MemoryAPI;

namespace EasyFarm.Tests.Context
{
    public class MockPlayer : IPlayer
    {
        public Status Status { get; set; }
        public int HppCurrent { get; set; }
        public bool HasAggro { get; set; }
        public Zone Zone { get; set; }
        public int Str { get; set; }
        public string Name { get; set; }
        public int MppCurrent { get; set; }
    }
}