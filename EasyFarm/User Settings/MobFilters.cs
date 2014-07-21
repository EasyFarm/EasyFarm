
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013 - 2014>  <Zerolimits>

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
using System.Text;

namespace EasyFarm.UserSettings
{
    /// <summary>
    /// Contains the data needed to filter out creatures.
    /// </summary>
    public class FilterInfo
    {        
        /// <summary>
        /// Name of the mob to be attacked
        /// </summary>
        public string TargetName = String.Empty;

        /// <summary>
        /// Name of the mob to be ignored
        /// </summary>
        public string IgnoredName = String.Empty;

        /// <summary>
        /// Used to filter out aggroed mobs.
        /// </summary>
        public bool AggroFilter = true;
        
        /// <summary>
        /// Used to filter out party claimed mobs.
        /// </summary>
        public bool PartyFilter = true;

        /// <summary>
        /// Used to filter out unclaimed mobs.
        /// </summary>
        public bool UnclaimedFilter = true;

        /// <summary>
        /// A list of mobs that we should ignore.
        /// </summary>
        public ObservableCollection<String> IgnoredMobs = new ObservableCollection<string>();

        /// <summary>
        /// A list of mobs that we should only kill.
        /// </summary>
        public ObservableCollection<String> TargetedMobs = new ObservableCollection<string>();
    }
}
