
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
using Parsing.Abilities;
using System;
using System.Linq;
using System.Threading;

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
            // Call for player to stop. 
            while (Player.IsMoving)
            {
                FFACE.Navigator.Reset();
                Thread.Sleep(100);
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
            var previous = FFACE.Player.CastPercentEx;
            var startTime = DateTime.Now;
            while (previous == FFACE.Player.CastPercentEx && DateTime.Now < startTime.AddSeconds(3))
            {
                FFACE.Windower.SendString(command);
                Thread.Sleep(100);
            }

            return true;
        }

        private bool MonitorCast()
        {
            // Monitor the cast and break when either the player 
            // moved or casting has been finished. 
            var position = FFACE.Player.Position;
            var castHistory = new LimitedQueue<short>(100);
            var prior = FFACE.Player.CastPercentEx;

            while (!castHistory.RepeatsN(30).Any())
            {
                if (prior == FFACE.Player.CastPercentEx)
                    castHistory.AddItem(FFACE.Player.CastPercentEx);

                // Has moved 
                if (position.X != FFACE.Player.PosX ||
                    position.Y != FFACE.Player.PosY ||
                    position.Z != FFACE.Player.PosZ) return false;                

                prior = FFACE.Player.CastPercentEx;

                Thread.Sleep(100);
            }

            // Report success
            if (castHistory.RepeatsN(30).Any(x => x.Equals(100))) return true;
            else return false;
        }

        public bool CastAbility(Ability ability)
        {           
            // Send the command to the game. 
            FFACE.Windower.SendString(ability.ToString());
            Thread.Sleep(100);
            return true;
        }

        public bool CastAbility(BattleAbility ability)
        {
            return CastAbility(ability.Ability);
        }
       
        public bool CastSpell(BattleAbility ability)
        {
            return CastSpell(ability.Ability);
        }
    }
}