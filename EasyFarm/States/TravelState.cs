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

using System.Collections.Generic;
using System.Linq;
using EasyFarm.Classes;
using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.States
{
    public class TravelState : BaseState
    {
        private Route Route => Config.Instance.Route;

        public TravelState(IMemoryAPI fface) : base(fface) { }

        public override bool Check()
        {
            // Waypoint list is empty.
            if (!Route.IsPathSet) return false;

            // Route belongs to a different zone.
            if (Route.Zone != fface.Player.Zone) return false;            

            // Has valid target to fight.
            if (UnitFilters.MobFilter(fface, CombatState.Target)) return false;

            // We don't have to rest. 
            if (new RestState(fface).Check()) return false;

            // We don't have to heal. 
            if (new HealingState(fface).Check()) return false;

            // We don't need to summon trusts
            if (new SummonTrustsState(fface).Check()) return false;

            // We are not bound or struck by an other movement
            // disabling condition. 
            if (ProhibitEffects.ProhibitEffectsMovement
                .Intersect(fface.Player.StatusEffects).Any())
                return false;

            return true;
        }

        public override void Run()
        {
            fface.Navigator.DistanceTolerance = 1;

            var nextPosition = Route.GetNextPosition(fface.Player.Position);
            var shouldKeepRunningToNextWaypoint = Config.Instance.Route.Waypoints.Count != 1;

            fface.Navigator.GotoWaypoint(
                nextPosition, 
                Config.Instance.IsObjectAvoidanceEnabled, 
                shouldKeepRunningToNextWaypoint);
        }

        public override void Exit()
        {
            fface.Navigator.Reset();
        }
    }
}