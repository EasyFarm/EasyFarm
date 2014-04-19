
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
    public class ActionBlocked
    {
        private Classes.GameEngine m_gameEngine;

        public ActionBlocked(ref Classes.GameEngine gameEngine)
        {
            this.m_gameEngine = gameEngine;
        }

        /// <summary>
        /// Returns true if we can not cast a spell.
        /// </summary>
        /// <returns></returns>
        public bool IsCastingBlocked
        {
            get
            {
                StatusEffect[] effectsThatBlock = 
            {
                StatusEffect.Silence,
                StatusEffect.Mute
            };

                // If we have effects that block,
                // return true.
                bool unableToCast = effectsThatBlock
                    .Intersect(m_gameEngine.FFInstance.Instance.Player.StatusEffects)
                    .Count() != 0;

                // 
                bool unableToReact = IsUnable;

                return unableToCast || unableToReact;
            }
        }

        /// <summary>
        /// Can we use job abilities?
        /// </summary>
        public bool IsAbilitiesBlocked
        {
            get
            {
                StatusEffect[] effectsThatBlock = 
            {
                StatusEffect.Amnesia
            };

                bool IsAbilitiesBlocked = effectsThatBlock
                    .Intersect(m_gameEngine.FFInstance.Instance.Player.StatusEffects)
                    .Count() != 0;

                return IsAbilitiesBlocked || IsUnable;
            }
        }

        /// <summary>
        /// Returns true if we have effects that inhibit us
        /// from taking any kind of action.
        /// </summary>
        /// <returns></returns>
        public bool IsUnable
        {
            get
            {
                StatusEffect[] effectsThatBlock = 
            {
                StatusEffect.Charm1, StatusEffect.Charm2, 
                StatusEffect.Petrification, StatusEffect.Sleep, 
                StatusEffect.Sleep2, StatusEffect.Stun, 
                StatusEffect.Chocobo, StatusEffect.Terror, 
            };

                bool IsPlayerUnable = effectsThatBlock
                    .Intersect(m_gameEngine.FFInstance.Instance.Player.StatusEffects)
                    .Count() != 0;

                return IsPlayerUnable;
            }
        }        
    }
}
