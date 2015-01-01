
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
            // Target dead or null.
            if (Target == null || Target.IsDead) return false;

            // We should approach mobs that have aggroed or have been pulled. 
            if (Target.Status.Equals(Status.Fighting)) return true;

            // Approach mobs if their distance is close. 
            return Target.Distance < 8;
        }

        public override void EnterComponent() { }

        public override void RunComponent()
        {
            // Move to target if out of melee range. 
            if (Target.Distance > Config.Instance.MiscSettings.MeleeDistance)
            {
                // Move to unit at max buff distance. 
                var oldTolerance = FFACE.Navigator.DistanceTolerance;
                FFACE.Navigator.DistanceTolerance = Config.Instance.MiscSettings.MeleeDistance;
                FFACE.Navigator.GotoNPC(Target.ID);
                FFACE.Navigator.DistanceTolerance = Config.Instance.MiscSettings.MeleeDistance;
            }

            // Face mob. 
            FFACE.Navigator.FaceHeading(Target.Position);

            // Target mob if not currently targeted. 
            if (Target.ID != FFACE.Target.ID)
            {
                // Set as target. 
                FFACE.Target.SetNPCTarget(Target.ID);
                FFACE.Windower.SendString("/ta <t>");
            }

            // Not engaged and in range. 
            if (!FFACE.Player.Status.Equals(Status.Fighting) && Target.Distance < 25)
            {
                // Engage the target. 
                FFACE.Windower.SendString(Constants.ATTACK_TARGET);
            }
        }

        public override void ExitComponent() { }
    }
}