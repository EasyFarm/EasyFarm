// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013 Mykezero
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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EasyFarm.Context;
using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.States
{
    public class Route
    {
        private int _position = -1;
        private Position _lastPosition;

        private List<Position> _positions = new List<Position>();
        public bool StraightRoute = true;
        public ObservableCollection<Position> Waypoints = new ObservableCollection<Position>();
        public Zone Zone { get; set; }

        public bool IsPathSet => Waypoints.Any();

        public void ResetCurrentWaypoint()
        {
            _position = -1;
        }

        public Position GetCurrentPosition(Position playerPosition)
        {
            _positions = Waypoints.ToList();

            if (_position == -1)
            {
                return GetNextPosition(playerPosition);
            }

            if (_position >= _positions.Count)
            {
                return null;
            }

            return _positions[_position];
        }

        public Position GetNextPosition(Position playerPosition)
        {
            _positions = Waypoints.ToList();

            if (_positions.Count <= 0)
            {
                return null;
            }

            if (_positions.Count < 2 && _position > -1)
            {
                return _positions[_position];
            }

            var closestPositions = _positions
                .OrderBy(x => Distance(playerPosition, x));

            var closest = closestPositions.FirstOrDefault();

            if (_position == -1)
            {
                _position = _positions.IndexOf(closest);
            }
            else if (_position == _positions.Count)
            {
                if (StraightRoute)
                {
                    Waypoints = new ObservableCollection<Position>(Waypoints.Reverse());
                    _positions.Reverse();
                    EasyFarm.ViewModels.LogViewModel.Write("Reached the end of waypoints; reversing.");
                }
                else
                {
                    EasyFarm.ViewModels.LogViewModel.Write("Reached the end of waypoints; circling.");
                }

                _position = 0;

            }

            var newPosition = _positions[_position];

            EasyFarm.ViewModels.LogViewModel.Write("Navigating to waypoint ("+_position+") " + newPosition.ToString());

            _position++;

            return newPosition;
        }

        private double Distance(Position one, Position other)
        {
            return Math.Sqrt(Math.Pow(one.X - other.X, 2) + Math.Pow(one.Z - other.Z, 2));
        }

        public void Reset()
        {
            _lastPosition = null;
            Waypoints.Clear();
            Zone = Zone.Unknown;
            _position = -1;
            _positions.Clear();
        }

        public bool IsWithinDistance(Position position, double distance)
        {
            return Waypoints.Any(x => Distance(position, x) <= distance);
        }

        public bool IsPathUnreachable(IGameContext context)
        {
            return Zone == context.Player.Zone &&
                context.NavMesh.FindPathBetween(context.API.Player.Position, GetCurrentPosition(context.API.Player.Position)).Count > 0;
        }
    }
}