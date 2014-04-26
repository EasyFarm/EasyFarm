
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013 - 2014>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*////////////////////////////////////////////////////////////////////

﻿using System;
using System.Linq;
using EasyFarm.Classes;
using FFACETools;

namespace EasyFarm.Classes
{
    public class UnitService
    {
        #region Members
        private static Unit[] UnitArray;
        private const short MOBARRAY_MAX = 768;
        private GameEngine _gameEngine;

        #endregion

        public UnitService(ref GameEngine gameEngine)
        {
            this._gameEngine = gameEngine;

            Unit.Session = gameEngine.Session.Instance;
            UnitArray = new Unit[MOBARRAY_MAX];

            // Create units
            for (int id = 0; id < MOBARRAY_MAX; id++)
            {
                UnitArray[id] = Unit.CreateUnit(id);
            }
        }

        #region Properties

        /// <summary>
        /// Does there exist a mob that has aggroed in general.
        /// </summary>
        public bool HasAggro
        {
            get
            {
                foreach (var Monster in ValidMobs)
                {
                    if (Monster.HasAggroed)
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
                foreach(var Monster in ValidMobs)
                {
                    if (Monster.IsClaimed)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public Unit[] ValidMobs
        {
            get
            {                
                return UnitArray
                    .Where(x => !x.Name.Equals(String.Empty))
                    .Where(x => IsValid(x))
                    .ToArray();
            }
        }

        public Unit GetTarget()
        {
                // Create a blank target
                Unit MainTarget = Unit.CreateUnit(0);

                // Create a copy of the valid mobs
                Unit[] PotentialTargets = ValidMobs;

                try
                {
                    return PotentialTargets
                            // Get all of the party claimed mobs
                            .Where(mob => _gameEngine.UserSettings.FilterInfo.PartyFilter && mob.PartyClaim)
                            .OrderBy(mob => mob.Distance)
                        .Concat(PotentialTargets
                            // Get all of my claimed mobs.
                            .Where(mob => mob.MyClaim)
                            .OrderBy(mob => mob.Distance))
                        .Concat(PotentialTargets
                            // Get all of the aggroed mobs
                            .Where(mob => _gameEngine.UserSettings.FilterInfo.AggroFilter && mob.HasAggroed)
                            .OrderBy(mob => mob.Distance))
                        .Concat(PotentialTargets
                            // Get all of the unclaimed mobs
                            .Where(mob => _gameEngine.UserSettings.FilterInfo.UnclaimedFilter && !mob.IsClaimed)
                            .OrderBy(mob => mob.Distance))                            
                        .First();                    
                }
                catch (InvalidOperationException)
                {
                    // Do Nothing, let bot retry
                }

                return MainTarget;
        }

        #endregion


        public bool IsValid(Unit unit)
        {
            // If what was passed in is null, its not valid.
            if (unit == null) return false;

            bool ValidMob =
                ((unit.IsActive) && (unit.Distance < 17) && (unit.YDifference < 5) && (unit.NPCBit != 0) && (!unit.IsDead) && (unit.NPCType == NPCType.Mob))
                &&
                (((_gameEngine.UserSettings.FilterInfo.TargetedMobs.Contains(unit.Name) && !unit.IsClaimed) || (_gameEngine.UserSettings.FilterInfo.TargetedMobs.Count == 0 && !_gameEngine.UserSettings.FilterInfo.IgnoredMobs.Contains(unit.Name)))
                ||
                ((unit.HasAggroed) || (unit.MyClaim) || (unit.PartyClaim)));

            return ValidMob;
        }
    }
}
