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

using System.Linq;
using System.Text.RegularExpressions;
using System;
using EasyFarm.Parsing;

namespace EasyFarm.Classes
{
    /// <summary>
    ///     This class is responsible for holding all of the abilities our character may use in battle.
    /// </summary>
    public class ActionFilters
    {
        /// <summary>
        ///     Filters out unusable buffing abilities.
        /// </summary>
        /// <param name="fface"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool BuffingFilter(MemoryWrapper fface, BattleAbility action)
        {
            // Return if not enabled. 
            if (!action.IsEnabled) return false;

            // Name Check
            if (string.IsNullOrWhiteSpace(action.Name)) return false;

            // MP Check
            if (action.Ability.MpCost > fface.Player.MPCurrent) return false;

            // TP Check
            if (action.Ability.TpCost > fface.Player.TPCurrent) return false;

            // Usage Limit Check. 
            if (action.UsageLimit != 0)
            {
                if (action.Usages > action.UsageLimit) return false;
            }

            // Recast Check
            if (!AbilityUtils.IsRecastable(fface, action.Ability)) return false;

            // Limiting Status Effect Check for Spells. 
            if (ResourceHelper.IsSpell(action.Ability.AbilityType))
            {
                if (ProhibitEffects.ProhibitEffectsSpell.Intersect(fface.Player.StatusEffects).Any())
                {
                    return false;
                }
            }

            // Limiting Status Effect Check for Abilities. 
            if (ResourceHelper.IsAbility(action.Ability.AbilityType))
            {
                if (ProhibitEffects.ProhibitEffectsAbility.Intersect(fface.Player.StatusEffects).Any())
                {
                    return false;
                }
            }

            // Player HP Checks Enabled. 
            if (action.PlayerLowerHealth != 0 || action.PlayerUpperHealth != 0)
            {
                // Player Upper HP Check
                if (fface.Player.HPPCurrent > action.PlayerUpperHealth) return false;

                // Player Lower HP Check
                if (fface.Player.HPPCurrent < action.PlayerLowerHealth) return false;
            }

            // Status Effect Checks Enabled
            if (!string.IsNullOrWhiteSpace(action.StatusEffect))
            {
                var hasEffect = fface.Player.StatusEffects.Any(effect =>
                    Regex.IsMatch(effect.ToString(),
                        action.StatusEffect.Replace(" ", "_"),
                        RegexOptions.IgnoreCase));

                // Contains Effect Check
                if (hasEffect && !action.TriggerOnEffectPresent) return false;

                // Missing EFfect Check
                if (!hasEffect && action.TriggerOnEffectPresent) return false;
            }

            // Check if action's recast period has passed. 
            if (action.Recast != 0)
            {
                if (action.LastCast > DateTime.Now) return false;
            }

            return true;
        }

        /// <summary>
        ///     Filters out unusable targeted abilities.
        /// </summary>
        /// <param name="fface"></param>
        /// <param name="action"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool TargetedFilter(MemoryWrapper fface, BattleAbility action, Unit unit)
        {
            // Does not pass the base criteria for casting. 
            if (!BuffingFilter(fface, action)) return false;

            // Target HP Checks Enabled. 
            if (action.TargetLowerHealth != 0 || action.TargetUpperHealth != 0)
            {
                // Target Upper Health Check
                if (unit.HppCurrent > action.TargetUpperHealth) return false;

                // Target Lower Health Check
                if (unit.HppCurrent < action.TargetLowerHealth) return false;
            }

            // Target Name Checks Enabled.
            if (!string.IsNullOrWhiteSpace(action.TargetName))
            {
                // Target Name Check. 
                if (!Regex.IsMatch(unit.Name, action.TargetName, RegexOptions.IgnoreCase)) return false;
            }

            // Distance Check
            if (unit.Distance > action.Distance) return false;

            return true;
        }
    }
}