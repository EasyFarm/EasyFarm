
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

namespace EasyFarm.Decision
{
    class TravelState : BaseState
    {
        int position = 0;

        public TravelState(ref GameEngine gameEngine) : base(ref gameEngine) { }

        public override bool CheckState()
        {
            return _engine.PlayerData.shouldTravel && _engine.IsWorking;
        }

        public override void EnterState()
        {
            _engine.RestingService.Off();
            _engine.CombatService.Disengage();

            _engine.Session.Instance.Navigator.DistanceTolerance = 1;
            _engine.Session.Instance.Navigator.HeadingTolerance = 1;
        }

        public override void RunState()
        {
            // If we've reached the end of the path....
            if (position > _engine.UserSettings.Waypoints.Count - 1)
            {
                // Turn around and run the path in reverse with the old end being the new starting point
                _engine.UserSettings.Waypoints = new ObservableCollection<Waypoint>(_engine.UserSettings.Waypoints.Reverse());
                position = 0;                
            }

            // If we are more than 10 yalms away from the nearest point...
            if (_engine.Session.Instance.Navigator.DistanceTo(_engine.UserSettings.Waypoints[position].Position) > 10)
            {
                SetPositionToNearestPoint();
            }

            _engine.Session.Instance.Navigator.Goto(_engine.UserSettings.Waypoints[position].Position, false);
            position++;
        }

        private void SetPositionToNearestPoint()
        {
            // Find the closest point and ...
            FFACE.Position closest = null;
            foreach (var current in _engine.UserSettings.Waypoints)
            {
                if (closest == null) { closest = current.Position; }

                else if (_engine.Session.Instance.Navigator.DistanceTo(current.Position) <
                    _engine.Session.Instance.Navigator.DistanceTo(closest))
                    closest = current.Position;
            }

            // Get its index in the array of points, then ...
            if (closest != null)
            {
                position = _engine.UserSettings.Waypoints.IndexOf(new Waypoint(closest));
            }
        }

        public override void ExitState()
        {
            _engine.Session.Instance.Navigator.Reset();
        }
    }
}
