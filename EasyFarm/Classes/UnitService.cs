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
using System;
using System.Collections.Generic;
using System.Linq;
using EasyFarm.UserSettings;
using MemoryAPI.Memory;

namespace EasyFarm.Classes
{
    /// <summary>
    /// Makes services available globally by providing build / create methods. 
    /// </summary>
    /// <remarks>
    /// Using this class to move the construction of services out of static methods.
    /// Classes here will eventually be moved somewhere else. 
    /// </remarks>
    public class GlobalFactory
    {
        public static Func<IMemoryAPI, IUnitService> BuildUnitService { get; set; }
            = eliteApi => new UnitService(eliteApi);

        public static IUnitService CreateUnitService(IMemoryAPI eliteApi)
        {
            return BuildUnitService.Invoke(eliteApi);
        }
    }

    /// <summary>
    /// Retrieves the zone's unit data.
    /// </summary>
    public class UnitService : IUnitService
    {
        private static bool _isInitialized;

        public UnitService(IMemoryAPI fface)
        {
            _fface = fface;

            if (_isInitialized) return;

            // Create the UnitArray
            Units = Enumerable.Range(0, UnitArrayMax)
                .Select(x => new Unit(_fface, x))
                .Cast<IUnit>().ToList();

            _isInitialized = true;
        }

        /// <summary>
        /// The zone's unit array.
        /// </summary>
        public static ICollection<IUnit> Units;

        /// <summary>
        /// The unit array's max size: 0 - 2048
        /// </summary>
        private const short UnitArrayMax = Constants.UnitArrayMax;

        /// <summary>
        /// The player's environmental data.
        /// </summary>
        private static IMemoryAPI _fface;

        /// <summary>
        /// Does there exist a mob that has aggroed in general.
        /// </summary>
        public bool HasAggro
        {
            get
            {
                var key = "HasAggro";
                var result = RuntimeCache.Get<bool?>(key);

                if (result.HasValue) return result.Value;
                var hasAggro = MobArray.Any(x => x.HasAggroed);

                RuntimeCache.Set(key, hasAggro, DateTimeOffset.Now.AddSeconds(Constants.UnitArrayCheckRate));
                return hasAggro;
            }
        }

        /// <summary>
        /// Retrieves the list of MOBs.
        /// </summary>
        public ICollection<IUnit> MobArray
        {
            get
            {
                return Units.Where(x => x.NpcType.Equals(NpcType.Mob)).ToList();
            }
        }

        /// <returns></returns>
        public IUnit GetUnitByName(string name)
        {
            return Units.FirstOrDefault(x => x.Name == name);
        }
    }
}