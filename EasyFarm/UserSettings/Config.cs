
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

﻿using EasyFarm.GameData;
using EasyFarm.ViewModels;
using FFACETools;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.UserSettings
{
    /// <summary>
    /// A configuration file for the user to edit through his GUI.
    /// Gives the bot access to all of his decisions.
    /// </summary>
    public class Config : BindableBase
    {
        /// <summary>
        /// Whether we are debugging this class. 
        /// </summary>
        public bool DebugEnabled { get; set; }

        [XmlIgnore]
        private static Lazy<Config> lazy = new Lazy<Config>(() => new Config());

        [XmlIgnore]
        public FFACE FFACE { get; set; }

        /// <summary>
        /// Sets up the default values for player settings. 
        /// Used also for unit testing purposes. 
        /// </summary>
        public Config()
        {
            this.ActionInfo = new ActionInfo();
            this.FilterInfo = new FilterInfo();
            this.MainWindowTitle = "Default";
            this.MiscSettings = new MiscSettings();
            this.RestingInfo = new RestingInfo();
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

        /// <summary>
        /// The text dislayed at the bottom of the screen
        /// </summary>
        [XmlIgnore]
        public String StatusBarText;

        /// <summary>
        /// List of all waypoints that make up the bots path
        /// </summary>
        public ObservableCollection<Waypoint> Waypoints;

        /// <summary>
        /// List of player usable moves. 
        /// </summary>
        public ActionInfo ActionInfo;

        /// <summary>
        /// List of options for filtering units. 
        /// </summary>
        public FilterInfo FilterInfo;

        /// <summary>
        /// Contains weaponskill info. 
        /// </summary>
        public WeaponSkill WeaponSkill;

        /// <summary>
        /// Contains information for resting.
        /// </summary>
        public RestingInfo RestingInfo;

        /// <summary>
        /// Contains the misc settings. 
        /// </summary>
        public MiscSettings MiscSettings;

        /// <summary>
        /// The window's name: player's name. 
        /// </summary>
        [XmlIgnore]
        public string MainWindowTitle;

        /// <summary>
        /// Saves the settings of Config object to file for later retrieval.
        /// </summary>
        /// <param name="Engine"></param>
        public void SaveSettings()
        {
            // Default file name for when fface is null. 
            string fileName = "UserPref.xml";

            // Get the filename in which to save the player's settings. 
            if (FFACE != null)
            {
                fileName = ViewModelBase.FFACE.Player.Name + "_UserPref.xml";
            }

            // Save all user settings under PlayerName_UserPref.xml. 
            Serialization.Serialize(fileName, Instance);
        }

        /// <summary>
        /// Loads the settings from the player specific configuration file to the Config obj.
        /// </summary>
        public void LoadSettings()
        {
            // Default file name when fface is null
            string fileName = "UserPref.xml";

            // Set filename to PlayerName_UserPref.xml when fface is found. 
            if (FFACE != null)
            {
                fileName = ViewModelBase.FFACE.Player.Name + "_UserPref.xml";
            }

            // Read the configuration from file for a given player. 
            // Note: this will wipe out your prevous fface instance for the object. 
            Instance = Serialization.Deserialize(fileName, Instance);

            // Reset the fface instance. 
            if (!DebugEnabled)
                Instance.FFACE = ViewModelBase.FFACE;
        }
    }
}