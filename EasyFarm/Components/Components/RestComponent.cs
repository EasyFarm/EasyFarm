
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
﻿
using EasyFarm.Classes;
using EasyFarm.UserSettings;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyFarm.Components
{
    /// <summary>
    /// Behavior for resting our character. 
    /// </summary>
    public class RestComponent : MachineComponent
    {
        private FFACE _fface;

        /// <summary>
        /// Retrieves aggroing creature. 
        /// </summary>
        private UnitService _units;

        /// <summary>
        /// The last time we checked for aggro. 
        /// </summary>
        private DateTime _lastAggroCheck = DateTime.Now;

        public RestComponent(FFACE fface)
        {
            this._fface = fface;
            this._units = new UnitService(fface);
        }

        public override bool CheckComponent()
        {
            // Check for aggro if possible; this check helps with program performance by limiting
            // constant checks against the whole unit array which is expensive. 
            if (_lastAggroCheck.AddSeconds(Constants.UNIT_ARRAY_CHECK_RATE) < DateTime.Now)
            {
                _lastAggroCheck = DateTime.Now;
                if (_units.HasAggro) return false;
            }

            // Check for effects taht stop resting. 
            if (ProhibitEffects.PROHIBIT_EFFECTS_DOTS
                .Intersect(_fface.Player.StatusEffects).Any()) return false;

            // Check if we are fighting. 
            if (_fface.Player.Status == Status.Fighting) return false;

            // Check if we should rest for health.
            if (Config.Instance.ShouldRestForHealth(
                _fface.Player.HPPCurrent,
                _fface.Player.Status)) return true;

            // Check if we should rest for magic. 
            if (Config.Instance.ShouldRestForMagic(
                _fface.Player.MPPCurrent,
                _fface.Player.Status)) return true;

            // We do not meet the conditions for resting. 
            return false;
        }

        public override void RunComponent()
        {
            RestingUtils.Rest(_fface);
        }

        /// <summary>
        /// Does our player have a status effect that prevents him
        /// </summary>
        /// <param name="playerStatusEffects"></param>
        /// <returns></returns>
        public bool IsRestingBlocked
        {
            get
            {
                var RestBlockingDebuffs = new List<StatusEffect>() 
            { 
                StatusEffect.Poison, StatusEffect.Bio, StatusEffect.Sleep, 
                StatusEffect.Sleep2, StatusEffect.Poison, StatusEffect.Petrification,
                StatusEffect.Stun, StatusEffect.Charm1, StatusEffect.Charm2, 
                StatusEffect.Terror, StatusEffect.Frost, StatusEffect.Burn, 
                StatusEffect.Choke, StatusEffect.Rasp, StatusEffect.Shock, 
                StatusEffect.Drown, StatusEffect.Dia, StatusEffect.Requiem, 
                StatusEffect.Lullaby
            };

                return RestBlockingDebuffs.Intersect(_fface.Player.StatusEffects).Count() != 0;
            }
        }
    }
}