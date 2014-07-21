
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
*////////////////////////////////////////////////////////////////////

﻿using FFACETools;
using ZeroLimits.FarmingTool;
using System.Linq;

namespace EasyFarm.State
{
    class HealingState : BaseState
    {
        public HealingState(FFACE fface) : base(fface) { }

        public override bool CheckState()
        {
            return FarmingTools.GetInstance(fface).PlayerData.shouldHeal && 
                !FarmingTools.GetInstance(fface).PlayerData.shouldRest;
        }

        public override void EnterState()
        {
            FarmingTools.GetInstance(fface).RestingService.Off();
        }

        public override void RunState()
        {
            // Use an ability to heal from the healing list if we can
            if (FarmingTools.GetInstance(fface).PlayerActions.HealingList.Count > 0)
            {
                // Check for actions available
                var act = FarmingTools.GetInstance(fface).PlayerActions.HealingList.FirstOrDefault();
                if (act == null) { return; }
                //
                else { FarmingTools.GetInstance(fface).AbilityExecutor.UseAbility(act); }
            }
        }

        public override void ExitState() { }
    }
}
