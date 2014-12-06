
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

using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyFarm.GameData;
using ZeroLimits.XITool;
using ZeroLimits.XITool.Classes;
using EasyFarm.UserSettings;
using ZeroLimits.XITool.Interfaces;
using ZeroLimits.XITool.Test;
using EasyFarm.State;

namespace ZeroLimits.FarmingTool
{
    /// <summary>
    /// This class is responsible for holding all of the abilities our character may use in battle.
    /// </summary>
    public class ActionFilters
    {
        /// <summary>
        /// Filters out usable abilities. 
        /// </summary>
        /// <param name="fface"></param>
        /// <returns></returns>
        public static Func<Ability, bool> AbilityFilter(FFACE fface)
        {
            return new Func<Ability, bool>((Ability x) => 
            {
                return new AbilityExecutor(fface).IsActionValid(x);
            });
        }

        /// <summary>
        /// Filters out usable healing abilities. 
        /// </summary>
        /// <param name="fface"></param>
        /// <returns></returns>
        public static Func<HealingAbility, bool> HealingFilter(FFACE fface)
        {
            return new Func<HealingAbility, bool>((HealingAbility x) =>
            {
                if(!x.IsEnabled) return false;

                if(x.TriggerLevel < fface.Player.HPPCurrent) return false;

                if(!AbilityFilter(fface)(new AbilityService().CreateAbility(x.Name))) return false;

                return true;
            });
        }

        /// <summary>
        /// A filter to checking whether we can use a weaponskill or not.  
        /// </summary>
        /// <param name="fface"></param>
        /// <returns></returns>
        public static Func<WeaponSkill, IUnit, bool> WeaponSkillFilter(FFACE fface)
        {
            //////////////////////////////////////////////////////
            // Code for testing weaponskill filtering.          //
            //////////////////////////////////////////////////////
            int TPCurrent = Constants.WEAPONSKILL_TP;
            Status Status = Status.Fighting;

            if (fface != null)
            {
                TPCurrent = fface.Player.TPCurrent;
                Status = fface.Player.Status;
            }
            //////////////////////////////////////////////////////

            return new Func<WeaponSkill, IUnit, bool>((WeaponSkill x, IUnit u) =>
            {
                // Weaponskill a valid ability?
                if (!x.Ability.IsValidName) return false;

                // Not enough tp. 
                if (TPCurrent < Constants.WEAPONSKILL_TP) return false;

                // Not engaged. 
                if (!Status.Equals(Status.Fighting)) return false;

                // The target's health is greater than the upper threshold, return false. 
                if (u.HPPCurrent> x.UpperHealth) return false;

                // Target's health is less than the lower threshold, return false. 
                if (u.HPPCurrent< x.LowerHealth) return false;

                // Do not meet distance requirements. 
                if (u.Distance > x.Distance) return false;

                return true;
            });
        }
    }
}
