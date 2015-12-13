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

using System.Linq;
using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.Components
{
    /// <summary>
    ///     Moves to target enemies.
    /// </summary>
    public class ApproachComponent : CombatBaseState
    {
        public ApproachComponent(FFACE fface) : base(fface) { }

        public override bool CheckComponent()
        {
            if (new RestComponent(FFACE).CheckComponent()) return false;

            // Target dead or null.
            if (!UnitFilters.MobFilter(FFACE, Target)) return false;

            // We should approach mobs that have aggroed or have been pulled. 
            if (Target.Status.Equals(Status.Fighting)) return true;

            // Get usable abilities. 
            var usable = Config.Instance.BattleLists["Pull"].Actions
                .Where(x => ActionFilters.BuffingFilter(FFACE, x));

            // Approach when there are no pulling moves available. 
            if (!usable.Any()) return true;

            // Approach mobs if their distance is close. 
            return Target.Distance < 8;
        }

        public override void RunComponent()
        {
            // Has the user decided that we should approach targets?
            if (Config.Instance.IsApproachEnabled)
            {
                // Move to target if out of melee range. 
                if (Target.Distance > Config.Instance.MeleeDistance)
                {
                    // Move to unit at max buff distance. 
                    FFACE.Navigator.DistanceTolerance = Config.Instance.MeleeDistance;
                    FFACE.Navigator.GotoNPC(Target.Id);
                }
            }

            // Face mob. 
            FFACE.Navigator.FaceHeading(Target.Position);

            // Target mob if not currently targeted. 
            if (Target.Id != FFACE.Target.ID)
            {
                // Set as target. 
                FFACE.Target.SetNPCTarget(Target.Id);
                FFACE.Windower.SendString("/ta <t>");
            }

            // Has the user decided we should engage in battle. 
            if (Config.Instance.IsEngageEnabled)
            {
                // Not engaged and in range. 
                if (!FFACE.Player.Status.Equals(Status.Fighting) && Target.Distance < 25)
                {
                    // Engage the target. 
                    FFACE.Windower.SendString(Constants.AttackTarget);
                }
            }
        }
    }
}