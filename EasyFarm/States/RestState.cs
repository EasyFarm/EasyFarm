
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
using FFACETools;
using System.Collections.Generic;
using ZeroLimits.FarmingTool;
using System.Linq;
using EasyFarm.ViewModels;
using EasyFarm.UserSettings;

namespace EasyFarm.States
{
    [StateAttribute(priority: 2)]
    public class RestState : BaseState
    {
        public RestState(FFACE fface) : base(fface) { }

        public override bool CheckState()
        {
            if (ftools.UnitService.HasAggro) return false;

            if (ftools.ActionBlocked.IsRestingBlocked) return false;

            if (FFACE.Player.Status == Status.Fighting) return false;

            if (Config.Instance.RestingInfo.Health
                .ShouldRest(FFACE.Player.HPPCurrent, FFACE.Player.Status)) return true;

            if (Config.Instance.RestingInfo.Magic
                .ShouldRest(FFACE.Player.MPPCurrent, FFACE.Player.Status)) return true;

            return false;
        }

        public override void EnterState() { }

        public override void RunState()
        {
            if (!FFACE.Player.Status.Equals(Status.Healing))
            {
                ftools.RestingService.StartResting();
            }
        }

        public override void ExitState() { }

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

                return RestBlockingDebuffs.Intersect(FFACE.Player.StatusEffects).Count() != 0;
            }
        }
    }
}