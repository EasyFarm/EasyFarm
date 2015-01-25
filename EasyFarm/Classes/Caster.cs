
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

using FFACETools;
using System;
using System.Linq;
using System.Threading;
using EasyFarm.Collections;
using ZeroLimits.XITool.Classes;

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
            if (!Helpers.IsRecastable(FFACE, ability)) return false;

            // Do we meet the mp requirements. 
            if (!Helpers.IsActionValid(FFACE, ability)) return false;

            // Call for player to stop. 
            while (Player.IsMoving)
            {
                FFACE.Navigator.Reset();
            }

            // Ensure command has been successfully sent. 
            int count = 0;            
            var previous = FFACE.Player.CastPercentEx;
            while (previous == FFACE.Player.CastPercentEx && count++ < 5 )
            {
                // Send the command to the game. 
                FFACE.Windower.SendString(ability.ToString());

                // Chainspelled spells will always be cast without fail. 
                if (FFACE.Player.StatusEffects.Contains(StatusEffect.Chainspell))
                    return true;
            }

            // Monitor the cast and break when either the player 
            // moved or casting has been finished. 
            var position = FFACE.Player.Position;
            var castHistory = new LimitedQueue<short>(100);

            while (!castHistory.RepeatsN(10).Any())
            {
                // Add item to history
                if (FFACE.Player.CastPercentEx == 0) continue;
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
            if (!Helpers.IsRecastable(FFACE, ability)) return false;

            // Do we meet the mp requirements. 
            if (!Helpers.IsActionValid(FFACE, ability)) return false;

            // Send the command to the game. 
            FFACE.Windower.SendString(ability.ToString());

            return true;
        }
    }
}