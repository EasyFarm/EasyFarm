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

using System;
using System.Linq;
using System.Threading;
using EasyFarm.Collections;
using EasyFarm.Parsing;
using MemoryAPI;

namespace EasyFarm.Classes
{
    /// <summary>
    ///     Monitors and Casts player abilities, spells and weaponskills.
    /// </summary>
    public class Caster
    {
        // Get player object. 
        private readonly IMemoryAPI _fface;

        public Caster(IMemoryAPI fface)
        {
            _fface = fface;
        }

        /// <summary>
        ///     Cast the spell and returns whether the cast was
        ///     successful or not.
        /// </summary>
        /// <param name="ability"></param>
        /// <returns></returns>
        public bool CastSpell(Ability ability)
        {
            // Try to cast the spell and return false if
            // we've failed to start casting or the 
            // casting was interrupted. 
            if (EnsureCast(ability.Command))
            {
                return MonitorCast();
            }

            return false;
        }

        /// <summary>
        ///     Ensures the command is sent to the game and
        ///     executed.
        /// </summary>
        private bool EnsureCast(string command)
        {
            // Chainspelled spells will always be cast without fail so 
            // cast it and return immediately. 
            if (_fface.Player.StatusEffects.Contains(StatusEffect.Chainspell))
            {
                _fface.Windower.SendString(command);
                return true;
            }

            // Ensure command has been successfully sent. 
            var previous = _fface.Player.CastPercentEx;
            var startTime = DateTime.Now;
            while (previous == _fface.Player.CastPercentEx && DateTime.Now < startTime.AddSeconds(3))
            {
                _fface.Windower.SendString(command);
                Thread.Sleep(100);
            }

            return true;
        }

        private bool MonitorCast()
        {
            // Monitor the cast and break when either the player 
            // moved or casting has been finished. 
            var position = _fface.Player.Position;
            var castHistory = new LimitedQueue<short>(100);
            var prior = _fface.Player.CastPercentEx;

            while (!castHistory.RepeatsN(30).Any())
            {
                if (prior == _fface.Player.CastPercentEx)
                    castHistory.AddItem(_fface.Player.CastPercentEx);

                // Has moved 
                if (position.X != _fface.Player.PosX ||
                    position.Y != _fface.Player.PosY ||
                    position.Z != _fface.Player.PosZ) return false;

                prior = _fface.Player.CastPercentEx;

                Thread.Sleep(100);
            }

            // Report success
            if (castHistory.RepeatsN(30).Any(x => x.Equals(100))) return true;
            return false;
        }

        public bool CastAbility(Ability ability)
        {
            // Send the command to the game. 
            _fface.Windower.SendString(ability.Command);
            Thread.Sleep(100);
            return true;
        }

        /// <summary>
        ///     Casts an ability with no monitoring.
        /// </summary>
        /// <param name="ability"></param>
        /// <returns></returns>
        public bool CastAbility(BattleAbility ability)
        {
            return CastAbility(ability.Ability);
        }

        /// <summary>
        ///     Casts a spell with monitoring.
        /// </summary>
        /// <param name="ability"></param>
        /// <returns></returns>
        public bool CastSpell(BattleAbility ability)
        {
            return CastSpell(ability.Ability);
        }
    }
}