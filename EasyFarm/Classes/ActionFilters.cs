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
using EasyFarm.ActionRules;
using MemoryAPI;

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
        public static bool BuffingFilter(IMemoryAPI fface, BattleAbility action)
        {
            var actionContext = new ActionContext
            {
                MemoryAPI = fface,
                BattleAbility = action
            };

            var actionRules = new BuffingActionRules();
            return actionRules.IsValid(actionContext);
        }

        /// <summary>
        ///     Filters out unusable targeted abilities.
        /// </summary>
        /// <param name="fface"></param>
        /// <param name="action"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool TargetedFilter(IMemoryAPI fface, BattleAbility action, IUnit unit)
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

            return true;
        }
    }
}
