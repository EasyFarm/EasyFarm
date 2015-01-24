
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

using System.Collections.Concurrent;

namespace EasyFarm.Collections
{
    /// <summary>
    /// A queue that keeps a list of a limited number of values. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LimitedQueue<T> : ConcurrentQueue<T>
    {
        /// <summary>
        /// The maximum limit the queue can hold. 
        /// </summary>
        public int Limit { get; set; }

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
        public void AddItem(T value)
        {
            // Used to discard an element. 
            T discard;

            // Remove the oldest value when limit is reached. 
            if (Count >= Limit) this.TryDequeue(out discard);

            // Add value to end.
            this.Enqueue(value);
        }
    }
}
