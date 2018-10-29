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
using MemoryAPI;
using EasyFarm.UserSettings;

namespace EasyFarm.Classes
{
    public interface IUnitFilters
    {
        /// <summary>
        /// Returns true if a mob is attackable by the player based on the various settings in the
        /// Config class.
        /// </summary>
        /// <param name="fface"></param>
        /// <param name="mob"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        bool MobFilter(IMemoryAPI fface, IUnit mob, IConfig config);
    }
}