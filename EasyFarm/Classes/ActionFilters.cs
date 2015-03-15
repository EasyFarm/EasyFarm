
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
using Parsing.Abilities;
using System;

namespace EasyFarm.Classes
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
        public static bool AbilityFilter(FFACE fface, Ability x)
        {
            return Helpers.IsActionValid(fface, x);
        }

        public static bool BattleAbilityFilter(FFACE fface, BattleAbility x)
        {
            // Return if not enabled. 
            if (!x.Enabled) return false;

            // Return if not castable.
            if (!x.IsCastable(fface)) return false;

            // If it meets these requirements, it's true. 
            // If it's a buff and has wore, true,
            // If it's not a buff then true. 
            return x.IsBuff() && x.HasEffectWore(fface) || !x.IsBuff();
        }

        /// <summary>
        /// Filters out usable healing abilities. 
        /// </summary>
        /// <param name="fface"></param>
        /// <returns></returns>
        public static bool HealingFilter(FFACE fface, HealingAbility x)
        {
            if (!x.IsEnabled)
                return false;

            if (x.TriggerLevel < fface.Player.HPPCurrent)
                return false;

            if (!AbilityFilter(fface, App.AbilityService.CreateAbility(x.Name)))
                return false;

            return true;
        }

        /// <summary>
        /// A filter to checking whether we can use a weaponskill or not.  
        /// </summary>
        /// <param name="fface"></param>
        /// <returns></returns>
        public static bool WeaponSkillFilter(FFACE fface, WeaponSkill x, IUnit u)
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

            // Weaponskill a valid ability?
            if (!x.Ability.IsValidName) return false;

            // Not enough tp. 
            if (TPCurrent < Constants.WEAPONSKILL_TP) return false;

            // Not engaged. 
            if (!Status.Equals(Status.Fighting)) return false;

            // The target's health is greater than the upper threshold, return false. 
            if (u.HPPCurrent >= x.UpperHealth) return false;

            // Target's health is less than the lower threshold, return false. 
            if (u.HPPCurrent <= x.LowerHealth) return false;

            // Do not meet distance requirements. 
            if (u.Distance >= x.Distance) return false;

            return true;
        }
    }
}
