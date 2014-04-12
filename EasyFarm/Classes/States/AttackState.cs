
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

namespace EasyFarm.FSM
{
    /// <summary>
    /// A class for defeating monsters.
    /// </summary>
    class AttackState : BaseState
    {
        public AttackState(ref GameEngine gameEngine) : base(ref gameEngine) { }

        public override bool CheckState()
        {
            return gameEngine.PlayerData.shouldFight;
        }

        public override void EnterState()
        {
            // gameEngine.Combat.Enter();
            gameEngine.Resting.Off();
        }

        public override void RunState()
        {
            gameEngine.Combat.Battle();
        }

        public override void ExitState()
        {
            //gameEngine.Combat.Exit();
        }
    }
}
