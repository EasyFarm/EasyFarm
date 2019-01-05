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
using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.Context
{
    public class Player : IPlayer
    {
        private readonly IMemoryAPI _memoryAPI;
        private readonly UnitService _unitService;

        public Player(IMemoryAPI memoryAPI)
        {
            _memoryAPI = memoryAPI;
            _unitService = new UnitService(memoryAPI);
        }

        public Status Status
        {
            get => _memoryAPI.Player.Status;
            set => throw new NotImplementedException();
        }

        public int HppCurrent
        {
            get => _memoryAPI.Player.HPPCurrent;
            set => throw new NotImplementedException();
        }

        public bool HasAggro
        {
            get => _unitService.HasAggro;
            set => throw new NotImplementedException();
        }

        public Zone Zone
        {
            get => _memoryAPI.Player.Zone;
            set => throw new NotImplementedException();
        }

        public int Str
        {
            get => _memoryAPI.Player.Stats.Str;
            set => throw new NotImplementedException();
        }

        public int MppCurrent
        {
            get => _memoryAPI.Player.MPPCurrent;
            set => throw new NotImplementedException();
        }
    }
}
