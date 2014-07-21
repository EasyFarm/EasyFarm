
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
*/
///////////////////////////////////////////////////////////////////

using FFACETools;
using ZeroLimits.FarmingTool;

namespace EasyFarm.State
{
    public class PostBattle : BaseState
    {
        public PostBattle(FFACE fface) : base(fface) { }

        public override bool CheckState()
        {
            return FarmingTools.GetInstance(fface)
                .TargetData.IsDead;
        }

        public override void EnterState() { }

        public override void RunState()
        {
            FarmingTools.GetInstance(fface).TargetData.TargetUnit = 
                FarmingTools.GetInstance(fface).UnitService.GetTarget();

            FarmingTools.GetInstance(fface).CombatService
                .ExecuteActions(FarmingTools.GetInstance(fface).PlayerActions.EndList);
        }

        public override void ExitState() { }
    }
}
