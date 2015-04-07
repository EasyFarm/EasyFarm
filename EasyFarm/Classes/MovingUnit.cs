
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

using EasyFarm.Collections;
using FFACETools;
using System;
using System.Linq;
using System.Timers;

namespace EasyFarm.Classes
{
    public class MovingUnit : Unit, IDisposable
    {
        /// <summary>
        /// Timer that ticks to calculate the current displacement, velocity and 
        /// acceleration from previous values. 
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// Create an object the timers can lock onto. 
        /// </summary>
        private object _mutex;

        private const int HISTORY_POSITION_LIMIT = 10;

        private ThresholdQueue<FFACE.Position> _positionHistory =
            new ThresholdQueue<FFACE.Position>(HISTORY_POSITION_LIMIT, .75);

        public bool IsVelocityEnabled { get; set; }

        public MovingUnit(FFACE fface, int id)
            : base(fface, id)
        {
            this._mutex = new object();
            this._timer = new Timer();
            this._timer.AutoReset = true;
            this._timer.Interval = 30;
            this._timer.Elapsed += TimerTick;
            this._timer.Start();
        }

        /// <summary>
        /// Updates our history of player positions, velocities and 
        /// sets the IsStuck field. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTick(object sender, ElapsedEventArgs e)
        {
            lock (_mutex)
            {
                _positionHistory.AddItem(this.Position);
            }
        }

        public bool IsStuck
        {
            get
            {
                return _positionHistory.IsThresholdMet(x => GetIsStuck(x));
            }
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
                var displacement = new FFACE.Position()
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

        public bool GetIsStuck(FFACE.Position velocity)
        {
            if (velocity.X == 0 && velocity.Z == 0) return false;
            if (velocity.X < .125 && velocity.Z < .250) return true;
            if (velocity.X < .250 && velocity.Z < .125) return true;
            return false;
        }

        public void Dispose()
        {
            this._timer.Dispose();
        }
    }
}
