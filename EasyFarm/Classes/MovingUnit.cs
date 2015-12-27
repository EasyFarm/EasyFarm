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

using System;
using System.Linq;
using System.Timers;
using EasyFarm.Collections;
using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.Classes
{
    /// <summary>
    ///     Information about a unit that can move.
    /// </summary>
    public class MovingUnit : Unit, IDisposable
    {
        private const int HistoryPositionLimit = 10;

        /// <summary>
        ///     Create an object the timers can lock onto.
        /// </summary>
        private readonly object _mutex;

        private readonly ThresholdQueue<Position> _positionHistory =
            new ThresholdQueue<Position>(HistoryPositionLimit, .75);

        /// <summary>
        ///     Timer that ticks to calculate the current displacement, velocity and
        ///     acceleration from previous values.
        /// </summary>
        private readonly Timer _timer;

        public MovingUnit(MemoryWrapper fface, int id)
            : base(fface, id)
        {
            _mutex = new object();
            _timer = new Timer
            {
                AutoReset = true,
                Interval = 30
            };
            _timer.Elapsed += TimerTick;
            _timer.Start();
        }
       
        public bool IsMoving
        {
            get
            {
                // Get the starting point. 
                var start = _positionHistory.FirstOrDefault();
                if (start == null) return false;

                // Get the end point. 
                var end = _positionHistory.LastOrDefault();
                if (end == null) return false;

                // Calculate the displacement
                var displacement = new Position
                {
                    X = start.X - end.X,
                    Y = start.Y - end.Y,
                    Z = start.Z - end.Z
                };

                // Return true if we've moved any direction. 
                return displacement.X != 0 ||
                       displacement.Y != 0 ||
                       displacement.Z != 0;
            }
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        /// <summary>
        ///     Updates our history of player positions, velocities and
        ///     sets the IsStuck field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTick(object sender, ElapsedEventArgs e)
        {
            lock (_mutex)
            {
                _positionHistory.AddItem(Position);
            }
        }
    }
}