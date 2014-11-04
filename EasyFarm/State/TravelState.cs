
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

﻿using System.Linq;
using System.Collections.ObjectModel;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroLimits.FarmingTool;
using EasyFarm.GameData;
using System.Diagnostics;
using EasyFarm.ViewModels;
using EasyFarm.UserSettings;


namespace EasyFarm.State
{
    class TravelState : BaseState
    {
        int position = 0;

        public TravelState(FFACE fface) : base(fface) { }

        public override bool CheckState()
        {
            if(Config.Instance.Waypoints.Count <= 0) return false;

            if(new AttackState(FFACE).CheckState()) return false;

            if(new RestState(FFACE).CheckState()) return false;

            if(new HealingState(FFACE).CheckState()) return false;

            if(ftools.ActionBlocked.IsUnable) return false;

            return true;
        }

        public override void EnterState()
        {
            ftools.RestingService.EndResting();
            ftools.CombatService.Disengage();

            FFACE.Navigator.DistanceTolerance = 1;
            FFACE.Navigator.HeadingTolerance = 1;
        }

        public override void RunState()
        {
            // If we've reached the end of the path....
            if (position > Config.Instance.Waypoints.Count - 1)
            {
                // Turn around and run the path in reverse with the old end being the new starting point
                Config.Instance.Waypoints =
                    new ObservableCollection<Waypoint>(Config.Instance.Waypoints.Reverse());
                position = 0;
            }

            // If we are more than 10 yalms away from the nearest point...
            if (FFACE.Navigator.DistanceTo(Config.Instance.Waypoints[position].Position) > 10)
            {
                SetPositionToNearestPoint();
            }

            bool isCanceled = false;
            var KeepFromRestingTask = new Task(() =>
            {
                while (!isCanceled)
                {
                    if (FFACE.Player.Status.Equals(Status.Healing))
                    {
                        ftools.RestingService.EndResting();
                    }
                }
            });

            KeepFromRestingTask.Start();
            FFACE.Navigator.Goto(Config.Instance.Waypoints[position].Position, false);
            isCanceled = true;
            position++;
        }

        private void SetPositionToNearestPoint()
        {
            // Find the closest point and ...
            FFACE.Position closest = null;
            foreach (var current in Config.Instance.Waypoints)
            {
                if (closest != null) Debug.WriteLine("Distance: " + FFACE.Navigator.DistanceTo(closest));

                if (closest == null) { closest = current.Position; }

                else if (FFACE.Navigator.DistanceTo(current.Position) <
                    FFACE.Navigator.DistanceTo(closest))
                    closest = current.Position;
            }

            // Get its index in the array of points, then ...
            if (closest != null)
            {
                Debug.WriteLine("Distance: " + FFACE.Navigator.DistanceTo(closest));
                position = Config.Instance.Waypoints.IndexOf(new Waypoint(closest));
            }
        }

        public override void ExitState()
        {
            FFACE.Navigator.Reset();
        }
    }
}
