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

namespace EasyFarm.States
{
    public class TravelState : AgentState
    {
        public TravelState(StateMemory stateMemory) : base(stateMemory)
        {
        }

        public override bool Check()
        {
            // Waypoint list is empty.
            if (!Config.Route.IsPathSet) return false;

            // Route belongs to a different zone.
            if (Config.Route.Zone != EliteApi.Player.Zone) return false;

            // Has valid target to fight.
            if (UnitFilters.MobFilter(EliteApi, Target, Config)) return false;

            // We don't have to rest.
            if (new RestState(Memory).Check()) return false;

            // We don't have to heal.
            if (new HealingState(Memory).Check()) return false;

            // We don't need to summon trusts
            if (new SummonTrustsState(Memory).Check()) return false;

            // We are not bound or struck by an other movement
            // disabling condition.
            if (ProhibitEffects.ProhibitEffectsMovement
                .Intersect(EliteApi.Player.StatusEffects).Any())
                return false;

            return true;
        }

        public override void Run()
        {
            EliteApi.Navigator.DistanceTolerance = 1;

            var nextPosition = Config.Route.GetNextPosition(EliteApi.Player.Position);
            var shouldKeepRunningToNextWaypoint = Config.Route.Waypoints.Count != 1;

            EliteApi.Navigator.GotoWaypoint(
                nextPosition,
                Config.IsObjectAvoidanceEnabled,
                shouldKeepRunningToNextWaypoint);
        }

        public override void Exit()
        {
            EliteApi.Navigator.Reset();
        }
    }
}