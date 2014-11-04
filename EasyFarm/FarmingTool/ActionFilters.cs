
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
    }
}
