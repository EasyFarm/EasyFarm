
/*///////////////////////////////////////////////////////////////////
Copyright (C) <Zerolimits>

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

using EasyFarm.Collections;
using FFACETools;
using System;
using System.Linq;

namespace EasyFarm.Classes
{
    public class Caster
    {
        // Get player object. 
        private static MovingUnit Player;
        private readonly FFACE FFACE;

        public Caster(FFACE fface)
        {
            this.FFACE = fface;
            MovingUnit.Session = this.FFACE;

            if (Player == null)
            {
                Player = new MovingUnit(FFACE.Player.ID);
            }
        }

        /// <summary>
        /// Cast the spell and returns whether the cast was 
        /// successful or not. 
        /// </summary>
        /// <param name="ability"></param>
        /// <returns></returns>
        public bool CastSpell(Ability ability)
        {
            // Player contains a move that prevents casting or movement. 
            if (ProhibitEffects.PROHIBIT_EFFECTS_SPELL.Intersect(FFACE.Player.StatusEffects).Any())
                return false;

            // Check if recast is available. 
            if (!Helpers.IsRecastable(FFACE, ability))
            {
                return false;
            }

            // Do we meet the mp requirements. 
            if (!Helpers.IsActionValid(FFACE, ability))
            {
                return false;
            }

            // Call for player to stop. 
            while (Player.IsMoving)
            {
                FFACE.Navigator.Reset();
            }

            // Try to cast the spell and return false if
            // we've failed to start casting or the 
            // casting was interrupted. 
            if (EnsureCast(ability.ToString()))
            {
                return MonitorCast();
            }
            
            return false;            
        }

        /// <summary>
        /// Ensures the command is sent to the game and 
        /// executed. 
        /// </summary>
        /// <param name="ability"></param>
        private bool EnsureCast(String command)
        {
            // Chainspelled spells will always be cast without fail so 
            // cast it and return immediately. 
            if (FFACE.Player.StatusEffects.Contains(StatusEffect.Chainspell))
            {
                FFACE.Windower.SendString(command);
                return true;
            }

            // Ensure command has been successfully sent. 
            int count = 0;
            var previous = FFACE.Player.CastPercentEx;
            while (previous == FFACE.Player.CastPercentEx && count++ < 5)
            {
                FFACE.Windower.SendString(command);
            }

            if (count == 5) return true;
            else return false;
        }

        private bool MonitorCast()
        {
            // Monitor the cast and break when either the player 
            // moved or casting has been finished. 
            var position = FFACE.Player.Position;
            var castHistory = new LimitedQueue<short>(100);

            while (!castHistory.RepeatsN(10).Any())
            {
                castHistory.AddItem(FFACE.Player.CastPercentEx);

                // Has moved 
                if (position.X != FFACE.Player.PosX ||
                    position.Y != FFACE.Player.PosY ||
                    position.Z != FFACE.Player.PosZ) return false;
            }

            // Report success
            if (castHistory.RepeatsN(10).Any(x => x.Equals(100))) return true;
            else return false;
        }

        public bool CastAbility(Ability ability)
        {
            // Check if recast is available. 
            if (!Helpers.IsRecastable(FFACE, ability))
            {
                return false;
            }

            // Do we meet the mp requirements. 
            if (!Helpers.IsActionValid(FFACE, ability))
            {
                return false;
            }

            // Send the command to the game. 
            FFACE.Windower.SendString(ability.ToString());
            return true;
        }

        public void CastAbility(BattleAbility ability)
        {
            this.CastAbility(ability.Ability);
        }
       
        public bool CastSpell(BattleAbility ability)
        {
            return this.CastSpell(ability.Ability);
        }
    }
}