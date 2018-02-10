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
        private readonly IConfigFactory _configFactory;

        public TravelState(StateMemory memory, IConfigFactory configFactory) : base(memory)
        {
            _configFactory = configFactory;
        }

        public override bool Check()
        {
            var config = _configFactory.GetConfig();

            // Waypoint list is empty.
            if (!config.Route.IsPathSet) return false;

            // Route belongs to a different zone.
            if (config.Route.Zone != EliteApi.Player.Zone) return false;

            // Has valid target to fight.
            if (UnitFilters.MobFilter(EliteApi, Target)) return false;

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
            var config = _configFactory.GetConfig();

            EliteApi.Navigator.DistanceTolerance = 1;

            var nextPosition = config.Route.GetNextPosition(EliteApi.Player.Position);
            var shouldKeepRunningToNextWaypoint = config.Route.Waypoints.Count != 1;

            EliteApi.Navigator.GotoWaypoint(
                nextPosition,
                config.IsObjectAvoidanceEnabled,
                shouldKeepRunningToNextWaypoint);
        }

        public override void Exit()
        {
            EliteApi.Navigator.Reset();
        }
    }
}