// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
// 
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////

using System.Linq;
using EasyFarm.Classes;
using EasyFarm.UserSettings;
using MemoryAPI;

namespace EasyFarm.States
{
    /// <summary>
    ///     Moves to target enemies.
    /// </summary>
    public class ApproachState : AgentState
    {
        public ApproachState(StateMemory memory) : base(memory)
        {
        }

        public override bool Check()
        {
            if (new RestState(Memory).Check()) return false;

            // Make sure we don't need trusts
            if (new SummonTrustsState(Memory).Check()) return false;

            // Target dead or null.
            if (!UnitFilters.MobFilter(EliteApi, Target)) return false;

            // We should approach mobs that have aggroed or have been pulled. 
            if (Target.Status.Equals(Status.Fighting)) return true;

            // Get usable abilities. 
            var usable = Config.Instance.BattleLists["Pull"].Actions
                .Where(x => ActionFilters.BuffingFilter(EliteApi, x));

            // Approach when there are no pulling moves available. 
            if (!usable.Any()) return true;

            // Approach mobs if their distance is close. 
            return Target.Distance < 8;
        }

        public override void Run()
        {
            // Has the user decided that we should approach targets?
            if (Config.Instance.IsApproachEnabled)
            {
                // Move to target if out of melee range. 
                EliteApi.Navigator.DistanceTolerance = Config.Instance.MeleeDistance;
                EliteApi.Navigator.GotoNPC(Target.Id, Config.Instance.IsObjectAvoidanceEnabled);
            }

            // Face mob. 
            EliteApi.Navigator.FaceHeading(Target.Position);

            // Target mob if not currently targeted. 
            Player.SetTarget(EliteApi, Target);

            // Has the user decided we should engage in battle. 
            if (Config.Instance.IsEngageEnabled)
                if (!EliteApi.Player.Status.Equals(Status.Fighting) && Target.Distance < 25)
                    EliteApi.Windower.SendString(Constants.AttackTarget);
        }
    }
}