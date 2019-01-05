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

using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.Classes
{
    public class NullUnit : IUnit
    {
        public int Id { get; set; }
        public int ClaimedId { get; }
        public double Distance { get; }
        public Position Position { get; } = new Position();
        public short HppCurrent { get; }
        public bool IsActive { get; }
        public bool IsClaimed { get; }
        public bool IsRendered { get; }
        public string Name { get; }
        public NpcType NpcType { get; }
        public float PosX { get; }
        public float PosY { get; }
        public float PosZ { get; }
        public Status Status { get; } = Status.Unknown;
        public bool MyClaim { get; }
        public bool HasAggroed { get; }
        public bool IsDead { get; }
        public bool PartyClaim { get; }
        public double YDifference { get; }
        public bool IsPet { get; }
        public bool IsValid { get; set; } = false;
    }
}