
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

using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ZeroLimits.XITool.Classes
{
    public class MovingUnit : Unit
    {
        /// <summary>
        /// Timer that ticks to calculate the current displacement, velocity and 
        /// acceleration from previous values. 
        /// </summary>
        private Timer timer;

        /// <summary>
        /// Create an object the timers can lock onto. 
        /// </summary>
        private object mutex;

        private const int HISTORY_POSITION_LIMIT = 10;
        private const int HISTORY_VELOCITY_LIMIT = 10;
        private const int HISTORY_OBSTRUCTED_LIMIT = 10;

        private LimitedQueue<FFACE.Position> positions =
            new LimitedQueue<FFACE.Position>(HISTORY_POSITION_LIMIT);

        private LimitedQueue<FFACE.Position> velocities =
            new LimitedQueue<FFACE.Position>(HISTORY_VELOCITY_LIMIT);

        private History obstructed =
            new History(HISTORY_OBSTRUCTED_LIMIT, .75);

        public bool IsVelocityEnabled { get; set; }

        public MovingUnit(int id)
            : base(id)
        {
            this.mutex = new object();
            this.timer = new Timer(); // Default interval is 1 second. 
            this.timer.AutoReset = true;
            this.timer.Interval = 30;
            this.timer.Elapsed += TimerTick;
            this.timer.Start();
        }

        public MovingUnit(Unit unit) : this(unit.ID) { }

        /// <summary>
        /// The current velocity.
        /// </summary>
        public FFACE.Position Velocity
        {
            get { return Ensure(velocities); }
        }

        /// <summary>
        /// Whether our player is moving or not. 
        /// </summary>
        public bool IsMoving
        {
            get
            {
                return Velocity.X != 0 && Velocity.Y != 0 && Velocity.Z != 0;
            }
        }

        /// <summary>
        /// Whether our player is stuck on a wall or not. 
        /// </summary>
        public bool IsStuck
        {
            get
            {
                return this.obstructed.Evaluate();
            }
        }

        /// <summary>
        /// Updates our history of player positions, velocities and 
        /// sets the IsStuck field. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTick(object sender, ElapsedEventArgs e)
        {
            lock (mutex)
            {
                // Add the current position to our positions queue. 
                this.positions.Add(this.Position);

                // If we have 10 position example to use.... 
                if (this.positions.Count > 10)
                {
                    var window = positions.Take(10);

                    // Calculate the displacements in the x, y and z directions. 
                    var velocity = CalculateDifferences(window.First(), window.Last());

                    // Store them in our displacment queue. 
                    this.velocities.Add(velocity);

                    // Add the velocity to determine if we are stuck. 
                    this.obstructed.Add(velocity);
                }
            }
        }

        /// <summary>
        /// Calculates the difference between points. 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private FFACE.Position CalculateDifferences(FFACE.Position p1, FFACE.Position p2)
        {
            return new FFACE.Position()
            {
                H = Math.Abs(p2.H - p1.H),
                X = Math.Abs(p2.X - p1.X),
                Y = Math.Abs(p2.Y - p1.Y),
                Z = Math.Abs(p2.Z - p1.Z)
            };
        }

        /// <summary>
        /// Ensure there is an FFACE.Position returned by the queues 
        /// and prevents an empty queue exception. 
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        private FFACE.Position Ensure(Queue<FFACE.Position> queue)
        {
            var value = new FFACE.Position();
            if (queue.Count > 0) value = queue.Peek();
            return value;
        }
    }
}
