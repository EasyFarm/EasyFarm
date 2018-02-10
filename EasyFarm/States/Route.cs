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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.States
{
    public class Route
    {
        private int _position;

        private List<Position> _positions = new List<Position>();
        public bool StraightRoute = true;
        public ObservableCollection<Position> Waypoints = new ObservableCollection<Position>();
        public Zone Zone { get; set; }

        public bool IsPathSet => Waypoints.Any();

        public Position GetNextPosition(Position playerPosition)
        {
            _positions = Waypoints.ToList();

            if (_position == _positions.Count)
            {
                if (StraightRoute)
                {
                    Waypoints = new ObservableCollection<Position>(Waypoints.Reverse());
                    _positions.Reverse();
                }

                _position = 0;
            }

            var distance = Distance(playerPosition, _positions[_position]);

            if (distance > 15)
            {
                var closest = _positions.OrderBy(x => Distance(playerPosition, x)).FirstOrDefault();
                _position = _positions.IndexOf(closest);
            }

            var newPosition = _positions[_position];

            _position++;

            return newPosition;
        }

        private double Distance(Position one, Position other)
        {
            return Math.Sqrt(Math.Pow(one.X - other.X, 2) + Math.Pow(one.Z - other.Z, 2));
        }

        public void Reset()
        {
            Waypoints.Clear();
            Zone = Zone.Unknown;
            _position = 0;
            _positions.Clear();
        }

        public bool IsWithinDistance(Position position, double distance)
        {
            return Waypoints.Any(x => Distance(position, x) <= distance);
        }

        public bool IsPathUnreachable(IMemoryAPI fface)
        {
            return Zone == fface.Player.Zone && IsWithinDistance(fface.Player.Position, 20);
        }
    }
}