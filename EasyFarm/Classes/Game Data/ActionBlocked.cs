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
