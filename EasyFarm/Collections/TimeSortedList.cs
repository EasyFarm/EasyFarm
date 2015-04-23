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
using System.Collections.Generic;
using System.Linq;

namespace EasyFarm.Collections
{
    /// <summary>
    ///     A list with a fixed limit that can return values within a
    ///     certain window of time.
    /// </summary>
    /// <typeparam name="V"></typeparam>
    public class TimeSortedList<V> : FixedSortedList<DateTime, V>
    {
        /// <summary>
        ///     Sets the max list size and window length.
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="window"></param>
        public TimeSortedList(int limit, int window) : base(limit)
        {
            Window = window;
        }

        /// <summary>
        ///     The window limit in terms of seconds.
        /// </summary>
        public int Window { get; set; }

        /// <summary>
        ///     Return all values within the window.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<V> GetValuesInWindow()
        {
            // Prevous: 1m
            // Prevous 1m 10s
            // Current: 1m 40s
            // We want chatlines that are within between 
            // 1m 30s and 1m 40s

            return this.Where(x => x.Key.AddSeconds(Window) >= DateTime.Now)
                .SelectMany(x => x.Value);
        }
    }
}