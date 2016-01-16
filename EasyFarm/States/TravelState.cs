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
using System.Collections.ObjectModel;
using System.Linq;
using EasyFarm.Classes;
using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.States
{
    public class TravelState : BaseState
    {
        private int _position;

        public TravelState(IMemoryAPI fface) : base(fface) { }

        public override bool CheckComponent()
        {
            // Waypoint list is empty.
            if (Config.Instance.Waypoints.Count <= 0) return false;

            // We are not able to attack any creatures. 
            if (new ApproachState(fface).CheckComponent()) return false;

            // We don't have to rest. 
            if (new RestState(fface).CheckComponent()) return false;

            // We don't have to heal. 
            if (new HealingState(fface).CheckComponent()) return false;

            // We are not bound or struck by an other movement
            // disabling condition. 
            if (ProhibitEffects.ProhibitEffectsMovement
                .Intersect(fface.Player.StatusEffects).Any())
                return false;

            return true;
        }

        public override void RunComponent()
        {
            // Navigator must be set by convention (other states could override)
            fface.Navigator.DistanceTolerance = 1;

            // Make a copy of the waypoint path from config. 
            var route = Config.Instance.Waypoints.ToList();

            // Reverse the waypoint path. 
            if (_position == route.Count)
            {
                if (Config.Instance.StraightRoute)
                {
                    Config.Instance.Waypoints = new ObservableCollection<Position>(Config.Instance.Waypoints.Reverse());
                    route = Config.Instance.Waypoints.ToList();
                }

                _position = 0;
            }

            // Run to the waypoint allowing cancellation on aggro or paused.             
            fface.Navigator.Goto(route[_position], false);
            _position++;
        }
    }
}