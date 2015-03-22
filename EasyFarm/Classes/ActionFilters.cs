
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
using System.Text.RegularExpressions;
using System.Linq;

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
        public static bool AbilityFilter(FFACE fface, Ability ability)
        {
            return Helpers.IsActionValid(fface, ability);
        }

        public static bool BattleAbilityFilter(FFACE fface, BattleAbility ability)
        {
            // Return if not enabled. 
            if (!ability.IsEnabled) return false;

            // Return if not castable.
            if (!Helpers.IsActionValid(fface, ability.Ability)) return false;

            var IsBuff = !String.IsNullOrWhiteSpace(ability.StatusEffect);

            var HasEffectWore = !fface.Player.StatusEffects.Any(effect =>
                Regex.IsMatch(effect.ToString(),
                ability.StatusEffect.Replace(" ", "_"),
                RegexOptions.IgnoreCase));

            // If it meets these requirements, it's true. 
            // If it's a buff and has wore, true,
            // If it's not a buff then true. 
            return IsBuff && HasEffectWore || !IsBuff;
        }

        /// <summary>
        /// Filters out usable healing abilities. 
        /// </summary>
        /// <param name="fface"></param>
        /// <returns></returns>
        public static bool HealingFilter(FFACE fface, BattleAbility x)
        {
            if (!x.IsEnabled) return false;

            if (x.PlayerLowerHealth < fface.Player.HPPCurrent && fface.Player.HPPCurrent > x.PlayerUpperHealth)
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
        public static bool WeaponSkillFilter(FFACE fface, BattleAbility x, IUnit u)
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
            if (string.IsNullOrWhiteSpace(x.Name)) return false;

            // Not enough tp. 
            if (TPCurrent < x.Ability.TPCost) return false;

            // Not engaged. 
            if (!Status.Equals(Status.Fighting)) return false;

            // The target's health is greater than the upper threshold, return false. 
            if (u.HPPCurrent > x.TargetUpperHealth) return false;

            // Target's health is less than the lower threshold, return false. 
            if (u.HPPCurrent < x.TargetLowerHealth) return false;

            // Do not meet distance requirements. 
            if (u.Distance > x.Distance) return false;

            return true;
        }
    }
}
