
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


using FFACETools;
using ZeroLimits.FarmingTool;

namespace EasyFarm.State
{
    /// <summary>
    /// A class for defeating monsters.
    /// </summary>
    class AttackState : BaseState
    {
        public AttackState(FFACE fface) : base(fface) { }

        public override bool CheckState()
        {
            return FarmingTools.GetInstance(fface)
                .PlayerData.shouldFight;
        }

        public override void EnterState()
        {
            FarmingTools.GetInstance(fface)
                .RestingService.Off();
        }

        public override void RunState()
        {
            FarmingTools.GetInstance(fface)
                .CombatService.Battle();
        }

        public override void ExitState()
        {

        }
    }
}
