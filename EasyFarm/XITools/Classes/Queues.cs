
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
using System.Collections.Generic;
using System.Linq;

namespace ZeroLimits.XITool.Classes
{
    /// <summary>
    /// A queue that only keeps a list of 10 values if the
    /// add method is used. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LimitedQueue<T> : Queue<T>
    {
        /// <summary>
        /// The maximum limit the queue can hold. 
        /// </summary>
        public int Limit { get; set; }

        public object mutex = new object();

        /// <summary>
        /// Create the object and set the maximum limit for the queue. 
        /// </summary>
        /// <param name="limit"></param>
        public LimitedQueue(int limit)
        {
            this.Limit = limit;
        }

        /// <summary>
        /// Add the object but only keep a set amount of them. 
        /// </summary>
        /// <param name="value"></param>
        public void Add(T value)
        {
            lock (mutex)
            {
                // Remove the oldest value when limit is reached. 
                if (Count > Limit) this.Dequeue();

                // Add value to end.
                this.Enqueue(value);
            }
        }
    }

    /// <summary>
    /// Maintains a history of values and has a function
    /// evaluate that returns true when a number of values
    /// is true. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ThresholdQueue<T> : LimitedQueue<T>
    {
        /// <summary>
        /// The percentage of examples that should return a true 
        /// for something to be true. 
        /// </summary>
        public double Threshold { get; set; }

        /// <summary>
        /// The function that determines whether something should 
        /// be true based on history and a threshold. 
        /// </summary>
        public abstract bool Evaluate();

        /// <summary>
        /// Create the object and set the maximum limit for the queue. 
        /// </summary>
        /// <param name="limit"></param>
        public ThresholdQueue(int limit, double threshold)
            : base(limit)
        {
            this.Threshold = threshold;
        }
    }

    public class History : ThresholdQueue<FFACE.Position>
    {
        public History(int limit, double threshold)
            : base(limit, threshold) { }

        /// <summary>
        /// Return whether there were enough trues to determine if we were
        /// really stuck. 
        /// </summary>
        /// <returns></returns>
        public override bool Evaluate()
        {
            lock (mutex)
            {
                var count = this.Where(x => GetIsStuck(x)).Count();
                var thresh = (int)(Limit * Threshold);
                return count >= thresh;
            }
        }

        public bool GetIsStuck(FFACE.Position velocity)
        {
            if (velocity.X == 0 && velocity.Z == 0) return false;
            if (velocity.X < .125 && velocity.Z < .250) return true;
            if (velocity.X < .250 && velocity.Z < .125) return true;
            return false;
        }
    }
}