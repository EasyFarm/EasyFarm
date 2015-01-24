
/*///////////////////////////////////////////////////////////////////
Copyright (C) <Zerolimits>

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

namespace EasyFarm.Collections
{
    /// <summary>
    /// Maintains a history of values and has a function
    /// evaluate that returns true when a number of values
    /// is true. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ThresholdQueue<T> : LimitedQueue<T>
    {
        /// <summary>
        /// The percentage of examples that should return a true 
        /// for something to be true. 
        /// </summary>
        public double Threshold { get; set; }

        /// <summary>
        /// Create the object and set the maximum limit for the queue. 
        /// </summary>
        /// <param name="limit"></param>
        public ThresholdQueue(int limit, double threshold)
            : base(limit)
        {
            this.Threshold = threshold;
        }

        /// <summary>
        /// Do the number of instances exceed or meet the threshold?
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool IsThresholdMet(Func<T, bool> condition)
        {
            return this.Where(x => condition(x)).Count() >= Math.Ceiling(Limit * Threshold);
        }
    }
}