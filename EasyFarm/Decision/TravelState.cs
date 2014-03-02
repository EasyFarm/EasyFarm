
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
using EasyFarm.Engine;
using System.Collections.ObjectModel;
using FFACETools;
using EasyFarm.UtilityTools;
using System;

namespace EasyFarm.FSM
{
    class TravelState : BaseState
    {
        int position = 0;
        DateTime runTime = DateTime.Now;

        public TravelState(ref GameEngine gameEngine) : base(ref gameEngine) { }

        public override bool CheckState()
        {
            return gameEngine.PlayerData.shouldTravel && gameEngine.IsWorking;
        }

        public override void EnterState()
        {
            gameEngine.Resting.Off();
            gameEngine.Combat.Disengage();

            gameEngine.FFInstance.Instance.Navigator.DistanceTolerance = 1;
            gameEngine.FFInstance.Instance.Navigator.HeadingTolerance = 1;
        }

        public override void RunState()
        {
            // If we've reached the end of the path....
            if (position >= gameEngine.Config.Waypoints.Count)
            {
                // Turn around and run the path in reverse with the old end being the new starting point
                gameEngine.Config.Waypoints = new ObservableCollection<FFACE.Position>(gameEngine.Config.Waypoints.Reverse());
                position = 0;
            }

            // If we are more than 10 yalms away from the nearest point...
            if (gameEngine.FFInstance.Instance.Navigator.DistanceTo(gameEngine.Config.Waypoints[position]) > 10)
            {
                // Find the closest point and ...
                FFACE.Position point = null;
                foreach (var pos in gameEngine.Config.Waypoints)
                {
                    if (point == null) { point = pos; }
                    
                    else if (gameEngine.FFInstance.Instance.Navigator.DistanceTo(pos) <
                        gameEngine.FFInstance.Instance.Navigator.DistanceTo(point))
                        point = pos;
                }

                // Get its index in the array of points, then ...
                if (point != null)
                {
                    position = gameEngine.Config.Waypoints.IndexOf(point);
                }
            }            

            // Run to the waypoint
            gameEngine.FFInstance.Instance.Navigator.Goto(gameEngine.Config.Waypoints[position], true, 10);
            // Set our position to the next point in the list
            position++;
        }

        public override void ExitState()
        {
            gameEngine.FFInstance.Instance.Navigator.Reset();
        }
    }
}
