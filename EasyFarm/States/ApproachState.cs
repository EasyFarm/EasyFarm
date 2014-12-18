
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
using EasyFarm.Logging;
using EasyFarm.UserSettings;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroLimits.FarmingTool;
using ZeroLimits.XITool;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.States
{
    /// <summary>
    /// Changes our target once the target becomes invalid
    /// </summary>    
    [StateAttribute(priority: 1)]
    public class ApproachState : BaseState
    {
        public Unit Target
        {
            get { return AttackState.TargetUnit; }
            set { AttackState.TargetUnit = value; }
        }

        public ApproachState(FFACE fface) : base(fface) { }

        public override bool CheckState()
        {
            return (Target != null && !AttackState.TargetUnit.IsDead &&
                Target.Distance > Config.Instance.MiscSettings.MeleeDistance);
        }

        public override void EnterState() { }

        public override void RunState()
        {
            FFACE.Navigator.Goto(Target.Position, false);
        }

        public override void ExitState() { }
    }
}