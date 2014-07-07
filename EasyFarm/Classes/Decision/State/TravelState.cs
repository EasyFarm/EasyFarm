
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
using EasyFarm.Classes;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyFarm.Classes.Services;

namespace EasyFarm.Decision
{
    class TravelState : BaseState
    {
        int position = 0;

        public TravelState(FFACE fface) : base(fface) { }

        public override bool CheckState()
        {
            return FarmingTools.GetInstance(fface).PlayerData.shouldTravel && 
                FarmingTools.GetInstance(fface).GameEngine.IsWorking;
        }

        public override void EnterState()
        {
            FarmingTools.GetInstance(fface).RestingService.Off();
            FarmingTools.GetInstance(fface).CombatService.Disengage();

            fface.Navigator.DistanceTolerance = 1;
            fface.Navigator.HeadingTolerance = 1;
        }

        public override void RunState()
        {
            // If we've reached the end of the path....
            if (position > FarmingTools.GetInstance(fface).UserSettings.Waypoints.Count - 1)
            {
                // Turn around and run the path in reverse with the old end being the new starting point
                FarmingTools.GetInstance(fface).UserSettings.Waypoints =
                    new ObservableCollection<Waypoint>(FarmingTools.GetInstance(fface).UserSettings.Waypoints.Reverse());
                position = 0;                
            }

            // If we are more than 10 yalms away from the nearest point...
            if (fface.Navigator.DistanceTo(FarmingTools.GetInstance(fface).UserSettings.Waypoints[position].Position) > 10)
            {
                SetPositionToNearestPoint();
            }

            fface.Navigator.Goto(FarmingTools.GetInstance(fface).UserSettings.Waypoints[position].Position, false);
            position++;
        }

        private void SetPositionToNearestPoint()
        {
            // Find the closest point and ...
            FFACE.Position closest = null;
            foreach (var current in FarmingTools.GetInstance(fface).UserSettings.Waypoints)
            {
                if (closest == null) { closest = current.Position; }

                else if (fface.Navigator.DistanceTo(current.Position) <
                    fface.Navigator.DistanceTo(closest))
                    closest = current.Position;
            }

            // Get its index in the array of points, then ...
            if (closest != null)
            {
                position = FarmingTools.GetInstance(fface).UserSettings.Waypoints.IndexOf(new Waypoint(closest));
            }
        }

        public override void ExitState()
        {
            fface.Navigator.Reset();
        }
    }
}
