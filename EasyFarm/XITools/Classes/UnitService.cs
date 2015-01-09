
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

namespace ZeroLimits.XITool.Classes
{
    public class UnitService
    {
        #region Members
        private static Unit[] _unitArray;
        private const short UNIT_ARRAY_MAX = Constants.UNIT_ARRAY_MAX;
        private static FFACE _fface;

        #endregion

        public UnitService(FFACE session)
        {
            _fface = session;
            Unit.Session = _fface;
            _unitArray = new Unit[UNIT_ARRAY_MAX];

            // Create units
            for (int id = 0; id < UNIT_ARRAY_MAX; id++)
            {
                _unitArray[id] = Unit.CreateUnit(id);
            }
        }

        #region Properties

        /// <summary>
        /// Used to filter units based on what the user thinks is valid. 
        /// Sets an optional filter to be used. 
        /// </summary>
        public Func<Unit, bool> UnitFilter { get; set; }

        /// <summary>
        /// Does there exist a mob that has aggroed in general.
        /// </summary>
        public bool HasAggro
        {
            get
            {
                foreach (var monster in FilteredArray)
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
                foreach (var monster in FilteredArray)
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
        /// Returns the list of filter units.
        /// </summary>
        public ICollection<Unit> FilteredArray
        {
            get
            {
                return _unitArray.Where(x => IsValid(x)).ToArray();
            }
        }

        /// <summary>
        /// Retrieves the list of UNITs
        /// </summary>
        public ICollection<Unit> UnitArray
        {
            get
            {
                return _unitArray;
            }
        }

        /// <summary>
        /// Retrieves the list of MOBs.
        /// </summary>
        public ICollection<Unit> MOBArray
        {
            get 
            {
                return UnitArray.Where(x => x.NPCType.Equals(NPCType.Mob)).ToArray();
            }
        }

        /// <summary>
        /// Retrieves the lsit of PCs.
        /// </summary>
        public ICollection<Unit> PCArray
        {
            get 
            {
                return UnitArray.Where(x => x.NPCType.Equals(NPCType.PC)).ToArray();
            }
        }

        #endregion

        /// <summary>
        /// Applies set filter to the unit array and 
        /// returns matches. 
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool IsValid(Unit unit)
        {
            return UnitFilter(unit);
        }

        /// <summary>
        /// Returns matched units that meet the filters requirements. 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public ICollection<Unit> GetUnits(Func<Unit, bool> filter)
        {
            return UnitArray.Where(filter).ToArray();
        }

        /// <summary>
        /// Returns matched units that meet the filters requirements 
        /// and then orders by the passed function.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public ICollection<Unit> GetUnits(Func<Unit, bool> filter, Func<Unit, object> orderby)
        {
            return UnitArray.Where(filter).OrderBy(orderby).ToArray();
        }

        /// <summary>
        /// Returns a single unit matching the filter. 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Unit GetTarget(Func<Unit, bool> filter)
        {
            return GetUnits(filter).FirstOrDefault();
        }

        /// <summary>
        /// Matches mobs against the "filter" and then orders them "orderby" 
        /// and returns the first match.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public Unit GetTarget(Func<Unit, bool> filter, Func<Unit, object> orderby)
        {
            return GetUnits(filter).OrderBy(orderby).FirstOrDefault();
        }

        // The default mob filter used in filtering the mobs. 
        public virtual Func<Unit, bool> DefaultFilter(FFACE fface)
        {
            throw new NotImplementedException("You need to override DefaultFilter with an implementation.");
        }
    }
}
