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
        public Zone Zone { get; set; }
        public ObservableCollection<Position> Waypoints = new ObservableCollection<Position>();
        private int _position;
        public bool StraightRoute = true;

        private List<Position> _positions = new List<Position>();

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