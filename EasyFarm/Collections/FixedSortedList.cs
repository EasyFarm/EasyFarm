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

using System.Collections.Generic;
using System.Linq;

namespace EasyFarm.Collections
{
    /// <summary>
    ///     A sorted list of a fixed size.
    /// </summary>
    public class FixedSortedList<K, V> : SortedList<K, List<V>>
    {
        /// <summary>
        ///     Create a new fixed size list with a limit.
        /// </summary>
        /// <param name="limit"></param>
        public FixedSortedList(int limit)
        {
            Limit = limit;
        }

        /// <summary>
        ///     Maximum capacity of the list.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        ///     Gets the number of elements contains in the FixedSortedList.
        /// </summary>
        public int CountItems
        {
            get { return this.Sum(x => x.Value.Count); }
        }

        /// <summary>
        ///     Adds a value but removes the oldest or
        ///     minimum value in the list.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddItem(K key, V value)
        {
            // Does not contain the key, create one. 
            if (!ContainsKey(key))
                this[key] = new List<V>();

            // Add a value to the key / values pairing. 
            this[key].Add(value);

            // Remove one chatline from the oldest list of chatlines. 
            if (CountItems > Limit && CountItems > 0)
            {
                // remove a single list item. 
                this.First().Value.Remove(this.First().Value.First());

                // List no longer contains items, remove it. 
                if (this.First().Value.Count == 0)
                {
                    Remove(this.First().Key);
                }
            }
        }
    }
}