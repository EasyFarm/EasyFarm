
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
*/
///////////////////////////////////////////////////////////////////

using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    /// <summary>
    /// A class for using abilties.
    /// </summary>

    public class AbilityExecutor
    {
        FFACE _fface;

        public AbilityExecutor(FFACE fface)
        {
            this._fface = fface;
        }

        /// <summary>
        /// Performs a list of actions. 
        /// Could be spells or job abilities. 
        /// Does not validate actions.
        /// </summary>
        /// <param name="actions"></param>
        /// <param name="unit"></param>
        public void ExecuteActions(List<Ability> actions, Action callback)
        {
            foreach (var act in actions)
            {
                callback();
                UseAbility(act);
            }
        }

        /// <summary>
        /// Attempts to use the passed in ability
        /// </summary>
        /// <param name="Ability"></param>
        public void UseAbility(Ability Ability)
        {
            // Set the duration to spell time or 50 for an ability
            int SleepDuration = Ability.IsSpell ? (int)Ability.CastTime + 1500 : 50;

            // Sleep for a second to pause the bots motion
            System.Threading.Thread.Sleep(1000);

            // Send it to the game
            _fface.Windower.SendString(Ability.ToString());

            // Sleep for the cast duration
            System.Threading.Thread.Sleep(SleepDuration);
        }
    }
}
