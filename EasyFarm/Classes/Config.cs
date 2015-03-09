
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

﻿using EasyFarm.Classes;
using EasyFarm.ViewModels;
using FFACETools;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace EasyFarm.UserSettings
{
    /// <summary>
    /// A configuration file for the user to edit through his GUI.
    /// Gives the bot access to all of his decisions.
    /// </summary>
    public partial class Config : BindableBase
    {
        /// <summary>
        /// Whether we are debugging this class. 
        /// </summary>
        public bool DebugEnabled { get; set; }

        [XmlIgnore]
        private static Lazy<Config> lazy = new Lazy<Config>(() => new Config());

        /// <summary>
        /// Sets up the default values for player settings. 
        /// Used also for unit testing purposes. 
        /// </summary>
        public Config()
        {
            this.MainWindowTitle = "Default";
            this.StatusBarText = String.Empty;
            this.Waypoints = new ObservableCollection<Waypoint>();
            this.WeaponSkill = new WeaponSkill();
        }

        [XmlIgnore]
        public static Config Instance
        {
            get
            {
                return lazy.Value;
            }
            set
            {
                lazy = new Lazy<Config>(() => value);
            }
        }
    }
}