
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
*////////////////////////////////////////////////////////////////////

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
    /// Gives the bot access to allow of his decisions.
    /// </summary>
    public class Config : BindableBase
    {
        [XmlIgnore]
        private static Lazy<Config> lazy = 
            new Lazy<Config>(() => new Config());

        [XmlIgnore]
        public FFACE FFACE { get; set; }

        private Config() { }

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
        public String StatusBarText = String.Empty;

        /// <summary>
        /// List of all waypoints that make up the bots path
        /// </summary>
        public ObservableCollection<Waypoint> Waypoints = new ObservableCollection<Waypoint>();

        /// <summary>
        /// List of player usable moves. 
        /// </summary>
        public ActionInfo ActionInfo = new ActionInfo();

        /// <summary>
        /// List of options for filtering units. 
        /// </summary>
        public FilterInfo FilterInfo = new FilterInfo();

        /// <summary>
        /// Contains weaponskill info. 
        /// </summary>
        public WeaponSkill WeaponSkill = new WeaponSkill();

        /// <summary>
        /// Contains information for resting.
        /// </summary>
        public RestingInfo RestingInfo = new RestingInfo();

        /// <summary>
        /// Contains the misc settings. 
        /// </summary>
        public MiscSettings MiscSettings = new MiscSettings();

        /// <summary>
        /// Saves the settings of Config object to file for later retrieval.
        /// </summary>
        /// <param name="Engine"></param>
        public void SaveSettings()
        {   
            String Filename = ViewModelBase.FFACE.Player.Name + "_UserPref.xml";
            Serialization.Serialize(Filename, Instance);
        }

        /// <summary>
        /// Loads the settings from the player specific configuration file to the Config obj.
        /// </summary>
        public void LoadSettings()
        {
            String Filename = ViewModelBase.FFACE.Player.Name + "_UserPref.xml";
            Instance = Serialization.Deserialize(Filename, Instance);
            Instance.FFACE = ViewModelBase.FFACE;
        }
    }
}