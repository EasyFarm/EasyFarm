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
using System.Collections.Generic;

namespace EasyFarm.Classes
{
    public interface IUnitService
    {
        /// <summary>
        /// Does there exist a mob that has aggroed in general.
        /// </summary>
        bool HasAggro { get; }

        /// <summary>
        /// Retrieves the list of MOBs.
        /// </summary>
        ICollection<IUnit> MobArray { get; }

        /// <summary>
        /// Retrieves a unit from the unit array by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IUnit GetUnitByName(string name);
    }
}