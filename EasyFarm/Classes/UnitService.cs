
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

﻿using System;
using System.Linq;
using FFACETools;
using System.Collections.Generic;

namespace EasyFarm.Classes
{
    /// <summary>
    /// Retrieves the zone's unit data. 
    /// </summary>
    public class UnitService
    {
        #region Members
        /// <summary>
        /// The zone's unit array. 
        /// </summary>
        private static IEnumerable<Unit> _units;

        /// <summary>
        /// The unit array's max size: 0 - 2048
        /// </summary>
        private const short UNIT_ARRAY_MAX = Constants.UNIT_ARRAY_MAX;

        /// <summary>
        /// The mob array's max size: 0 - 768.
        /// </summary>
        private const short MOB_ARRAY_MAX = Constants.MOB_ARRAY_MAX;

        /// <summary>
        /// The player's environmental data. 
        /// </summary>
        private static FFACE _fface;
        #endregion

        public UnitService(FFACE fface)
        {
            _fface = fface;

            // Create the UnitArray
            _units = Enumerable.Range(0, UNIT_ARRAY_MAX)
                .Select(x => new Unit(_fface, x));
        }

        #region Properties

        /// <summary>
        /// Does there exist a mob that has aggroed in general.
        /// </summary>
        public bool HasAggro
        {
            get
            {
                foreach (var monster in MOBArray)
                {
                    if (monster.HasAggroed)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Do we have claim on any mob?
        /// </summary>
        public bool HasClaim
        {
            get
            {
                foreach (var monster in MOBArray)
                {
                    if (monster.IsClaimed)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Retrieves the list of UNITs
        /// </summary>
        public IEnumerable<Unit> UnitArray
        {
            get
            {
                return _units;
            }
        }

        /// <summary>
        /// Retrieves the list of MOBs.
        /// </summary>
        public IEnumerable<Unit> MOBArray
        {
            get
            {
                return UnitArray.Take(MOB_ARRAY_MAX)
                    .Where(x => x.NPCType.Equals(NPCType.Mob));
            }
        }

        /// <summary>
        /// Retrieves the lsit of PCs.
        /// </summary>
        public IEnumerable<Unit> PCArray
        {
            get
            {
                return UnitArray.Skip(MOB_ARRAY_MAX)
                    .Where(x => x.NPCType.Equals(NPCType.PC));
            }
        }

        #endregion
    }
}
