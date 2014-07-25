
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013 - 2014>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*////////////////////////////////////////////////////////////////////

﻿using System.Linq;
using System.Collections.ObjectModel;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroLimits.FarmingTool;
using EasyFarm.GameData;
using System.Diagnostics;


namespace EasyFarm.State
{
    class TravelState : BaseState
    {
        int position = 0;

        public TravelState(FFACE fface) : base(fface) { }

        public override bool CheckState()
        {
            return ftools.PlayerData.shouldTravel;
        }

        public override void EnterState()
        {
            ftools.RestingService.EndResting();
            ftools.CombatService.Disengage();

            fface.Navigator.DistanceTolerance = 1;
            fface.Navigator.HeadingTolerance = 1;
        }

        public override void RunState()
        {
            // If we've reached the end of the path....
            if (position > ftools.UserSettings.Waypoints.Count - 1)
            {
                // Turn around and run the path in reverse with the old end being the new starting point
                ftools.UserSettings.Waypoints =
                    new ObservableCollection<Waypoint>(ftools.UserSettings.Waypoints.Reverse());
                position = 0;                
            }

            // If we are more than 10 yalms away from the nearest point...
            if (fface.Navigator.DistanceTo(ftools.UserSettings.Waypoints[position].Position) > 10)
            {
                SetPositionToNearestPoint();
            }

            bool isCanceled = false;
            var KeepFromRestingTask = new Task(() => {
                while (!isCanceled)
                {
                    if (fface.Player.Status.Equals(Status.Healing))
                    {
                        ftools.RestingService.EndResting();
                    }
                }
            });

            KeepFromRestingTask.Start();
            fface.Navigator.Goto(ftools.UserSettings.Waypoints[position].Position, true);
            isCanceled = true;
            position++;
        }

        private void SetPositionToNearestPoint()
        {
            // Find the closest point and ...
            FFACE.Position closest = null;
            foreach (var current in ftools.UserSettings.Waypoints)
            {
                if(closest != null) Debug.WriteLine("Distance: " + fface.Navigator.DistanceTo(closest));

                if (closest == null) { closest = current.Position; }

                else if (fface.Navigator.DistanceTo(current.Position) <
                    fface.Navigator.DistanceTo(closest))
                    closest = current.Position;
            }

            // Get its index in the array of points, then ...
            if (closest != null)
            {
                Debug.WriteLine("Distance: " + fface.Navigator.DistanceTo(closest));
                position = ftools.UserSettings.Waypoints.IndexOf(new Waypoint(closest));
            }
        }

        public override void ExitState()
        {
            fface.Navigator.Reset();
        }
    }
}
