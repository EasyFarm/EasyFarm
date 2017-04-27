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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EasyFarm.Classes
{
    /// <summary>
    ///     Class for finding battle lists by name.
    /// </summary>
    public class BattleLists : ObservableCollection<BattleList>
    {
        public BattleLists()
        {
        }

        public BattleLists(List<BattleList> list) : base(list)
        {
        }

        public BattleLists(IEnumerable<BattleList> collection) : base(collection)
        {
        }

        public IEnumerable<BattleAbility> Actions
        {
            get { return this.SelectMany(x => x.Actions); }
        }

        /// <summary>
        ///     Access a battle list by its name.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public BattleList this[string index]
        {
            get
            {
                // Find the list item with that name. 
                var list = this.FirstOrDefault(x => x.Name.Equals(index));

                // Throw error if now found. 
                if (list == null)
                    throw new Exception(
                        string.Format("No key {0} in battle lists to get value. ", index)
                        );

                // Return the list matching the name. 
                return list;
            }
            set
            {
                // Find the list item with that name. 
                var list = this.FirstOrDefault(x => x.Name.Equals(index));

                // Throw error when key not found.
                if (list == null)
                    throw new Exception(
                        string.Format("No key {0} in battle lists to set value. ", index)
                        );

                // Remove the old reference to the indexed value. 
                Remove(list);

                // Add the new indexed value. 
                Add(value);
            }
        }
    }
}