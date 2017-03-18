/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

using MemoryAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyFarm.Classes
{
    /// <summary>
    /// Retrieves the zone's unit data.
    /// </summary>
    public class UnitService
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

        // We are caching values for HasAggro, since the program will call it constantly which will
        // result in a performance jump.

        /// <summary>
        /// The last time an aggro check was performed.
        /// </summary>
        public DateTime LastAggroCheck = DateTime.Now;

        /// <summary>
        /// The last value read from HasAggro
        /// </summary>
        public bool LastAggroStatus;

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
                // Return the cached value if we're checking too often.
                if (LastAggroCheck.AddSeconds(
                    Constants.UnitArrayCheckRate) >
                    DateTime.Now)
                {
                    return LastAggroStatus;
                }

                // Update the last checked time.
                LastAggroCheck = DateTime.Now;

                // Otherwise return the current environment value.
                return LastAggroStatus = MobArray.Any(x => x.HasAggroed);
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
    }
}