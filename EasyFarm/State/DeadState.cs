
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
*////////////////////////////////////////////////////////////////////

﻿
using FFACETools;
using ZeroLimits.FarmingTool;

namespace EasyFarm.State
{
    /// <summary>
    /// A state to pause the bot if it is dead.
    /// </summary>
    class DeadState : BaseState
    {
        public DeadState(FFACE fface) : base(fface) { }

        public override bool CheckState() { return ftools.PlayerData.IsDead; }

        public override void EnterState() { }

        public override void RunState() {
            // Stop the finite state machine from 
            // processing  more states. 
            App.GameEngine.Stop();
            
            // Inform the user through the status bar 
            // we've stopped. 
            App.InformUser("Stopped!");
            
            // Stop moving towards waypoints if we 
            // were currently moving.
            fface.Navigator.Reset();
        }

        public override void ExitState() { }
    }
}