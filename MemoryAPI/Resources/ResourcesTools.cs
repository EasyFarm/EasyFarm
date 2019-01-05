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
using EliteMMO.API;

namespace MemoryAPI.Resources
{
    public class ResourcesTools : IResourcesTools
    {
        private readonly EliteAPI _api;

        public ResourcesTools(EliteAPI api)
        {
            _api = api;
        }

        public EliteAPI.ISpell GetSpell(Int32 index)
        {
            return _api.Resources.GetSpell((UInt32)index);
        }

        public EliteAPI.IAbility GetAbility(Int32 index)
        {
            return _api.Resources.GetAbility((UInt32)index);
        }

        public EliteAPI.IItem GetItem(Int32 index)
        {
            return _api.Resources.GetItem((UInt32)index);
        }
    }
}
