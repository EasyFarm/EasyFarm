
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
using EasyFarm.Engine;
using FFACETools;

namespace EasyFarm.UnitTools
{
    public class Units
    {
        #region Members

        private static Unit[] UnitArray;
        private static GameEngine Engine;
        private const short MOBARRAY_MAX = 768;

        #endregion

        #region Properties

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

        public Unit Target
        {
            get
            {
                // Create a blank target
                var MainTarget = Unit.CreateUnit(0);

                // Create a copy of the valid mobs
                Unit[] PotentialTargets = ValidMobs;

                try
                {
                    if (Engine.Config.BattlePartyClaimed && PotentialTargets.Where(mob => mob.PartyClaim).Count() > 0)
                    {
                        MainTarget = PotentialTargets.OrderBy(x=> x.Distance).First(mob => mob.PartyClaim);
                    }
                    else if (PotentialTargets.Where(mob => mob.MyClaim).Count() > 0)
                    {
                        MainTarget = PotentialTargets.OrderBy(x => x.Distance).First(mob => mob.MyClaim);
                    }
                    else if (Engine.Config.BattleAggro && PotentialTargets.Where(mob => mob.HasAggroed).Count() > 0)
                    {
                        MainTarget = PotentialTargets.OrderBy(x => x.Distance).First(mob => mob.HasAggroed);
                    }
                    else if (Engine.Config.BattleUnclaimed && PotentialTargets.Where(mob => !mob.IsClaimed).Count() > 0)
                    {
                        MainTarget = PotentialTargets.OrderBy(x => x.Distance).Where(mob => !mob.IsClaimed).First();
                    }
                }
                catch (InvalidOperationException)
                {
                    // Do Nothing, let bot retry
                }

                return MainTarget;
            }
        }

        #endregion

        #region Constructors

        public Units(ref GameEngine GameEngine)
        {
            Engine = GameEngine;
            Unit.Session = Engine.FFInstance.Instance;
            UnitArray = new Unit[MOBARRAY_MAX];

            // Create units
            for (int id = 0; id < MOBARRAY_MAX; id++)
            {
                UnitArray[id] = Unit.CreateUnit(id);
            }
        }

        public bool IsValid(Unit unit)
        {
            // If what was passed in is null, its not valid.
            if (unit == null) return false; 

            bool ValidMob =
                ((unit.IsActive) && (unit.Distance < 17) && (unit.YDifference < 5) && (unit.NPCBit != 0) && (!unit.IsDead) && (unit.NPCType == NPCType.Mob))
                &&
                (((Engine.Config.TargetsList.Contains(unit.Name) && !unit.IsClaimed) || (Engine.Config.TargetsList.Count == 0 && !Engine.Config.IgnoredList.Contains(unit.Name)))
                ||
                ((unit.HasAggroed) || (unit.MyClaim) || (unit.PartyClaim)));

            return ValidMob;
        }

        #endregion
    }
}
