
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

﻿using EasyFarm.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.FSM
{
    class HealingState : BaseState
    {
        public HealingState(ref GameEngine gameEngine) : base(ref gameEngine) { }

        public override bool CheckState()
        {
            return _engine.PlayerData.shouldHeal && !_engine.PlayerData.shouldRest;
        }

        public override void EnterState()
        {
            _engine.RestingService.Off();
        }

        public override void RunState()
        {
            // Use an ability to heal from the healing list if we can
            if(_engine.PlayerActions.HealingList.Count > 0)
            {
                // Check for actions available
                var act = _engine.PlayerActions.HealingList.FirstOrDefault();
                if (act == null) { return; }
                //
                else { _engine.AbilityExecutor.UseAbility(act); }
            }
        }

        public override void ExitState() { }
    }
}
