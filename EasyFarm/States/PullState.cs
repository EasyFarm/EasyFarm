
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

using EasyFarm.FarmingTool;
using FFACETools;
using System;
using ZeroLimits.FarmingTool;
using ZeroLimits.XITool.Classes;
using System.Linq;
using EasyFarm.ViewModels;
using EasyFarm.UserSettings;
using EasyFarm.Logging;

namespace EasyFarm.States
{
    [StateAttribute(priority: 2)]
    public class PullState : BaseState
    {
        public Unit Target
        {
            get { return AttackState.TargetUnit; }
            set { AttackState.TargetUnit = value; }
        }

        public PullState(FFACE fface) : base(fface) { }

        public override bool CheckState()
        {
            return !AttackState.fightStarted && !Target.Status.Equals(Status.Fighting) && !Target.IsDead;
        }

        public override void EnterState() { }

        public override void RunState()
        {
            // Pull the target casting each spell once until the target is claimed
            var UsablePullingMoves = Config.Instance.ActionInfo.PullList
                .Where(x => ActionFilters.AbilityFilter(FFACE)(x))
                .ToList();

            ftools.AbilityExecutor.ExecuteActions(UsablePullingMoves);
        }

        public override void ExitState()
        {
            if (Target.Status.Equals(Status.Fighting))
            {
                AttackState.fightStarted = true;
            }
        }
    }
}
