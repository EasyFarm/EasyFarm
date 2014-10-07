
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

﻿using FFACETools;
using ZeroLimits.FarmingTool;
using System.Linq;
using ZeroLimits.XITools;

namespace EasyFarm.State
{
    class HealingState : BaseState
    {
        public HealingState(FFACE fface) : base(fface) { }

        public override bool CheckState()
        {
            return ftools.PlayerData.shouldHeal &&
                !ftools.PlayerData.shouldRest;
        }

        public override void EnterState()
        {
            ftools.RestingService.EndResting();
        }

        public override void RunState()
        {
            // Use an ability to heal from the healing list if we can
            if (ftools.PlayerActions.HealingList.Count > 0)
            {
                // Check for actions available
                var act = ftools.PlayerActions.HealingList.FirstOrDefault();
                if (act == null) { return; }
                //
                else { ftools.AbilityExecutor.UseAbility(act, Constants.SPELL_CAST_LATENCY, Constants.GLOBAL_SPELL_COOLDOWN); }
            }
        }

        public override void ExitState() { }
    }
}
