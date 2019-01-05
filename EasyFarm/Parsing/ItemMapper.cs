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
using System.Linq;
using EliteMMO.API;

namespace EasyFarm.Parsing
{
    public class ItemMapper
    {
        public Ability Map(EliteAPI.IItem item)
        {
            return new Ability()
            {
                CastTime = item.CastTime,
                English = item.Name?.FirstOrDefault() ?? "",
                Index = (Int32) item.ItemID,
                Prefix = "/item",
                Recast = item.RecastDelay,
                TargetType = (TargetType) item.ValidTargets,
                AbilityType = AbilityType.Item
            };
        }
    }
}