
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

using EasyFarm.Components;
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

namespace EasyFarm.Components
{
    /// <summary>
    /// Moves to target enemies. 
    /// </summary>
    public class ApproachComponent : MachineComponent
    {
        public FFACE FFACE { get; set; }

        public Unit Target
        {
            get { return AttackContainer.TargetUnit; }
            set { AttackContainer.TargetUnit = value; }
        }

        public ApproachComponent(FFACE fface)
        {
            this.FFACE = fface;
        }

        public override bool CheckComponent()
        {
            return (Target != null && !AttackContainer.TargetUnit.IsDead &&
                Target.Distance > Config.Instance.MiscSettings.MeleeDistance);
        }

        public override void EnterComponent() { }

        public override void RunComponent()
        {
            var OldTolerance = FFACE.Navigator.DistanceTolerance;
            FFACE.Navigator.DistanceTolerance = Config.Instance.MiscSettings.MeleeDistance;

            FFACE.Navigator.Goto(() => Target.PosX, () => Target.PosZ, false);

            FFACE.Navigator.DistanceTolerance = OldTolerance;
        }

        public override void ExitComponent() {  }
    }
}