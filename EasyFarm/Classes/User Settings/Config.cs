
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
*////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections.Generic;
using FFACETools;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using EasyFarm.Classes;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Prism.Mvvm;
using EasyFarm.Classes.User_Settings;

namespace EasyFarm.Classes
{
    /// <summary>
    /// A configuration file for the user to edit through his GUI.
    /// Gives the bot access to allow of his decisions.
    /// </summary>
    public class Config : BindableBase
    {                       
        /// <summary>
        /// The text dislayed at the bottom of the screen
        /// </summary>
        [XmlIgnore]
        public String StatusBarText = String.Empty;

        /// <summary>
        /// List of all waypoints that make up the bots path
        /// </summary>
        public ObservableCollection<Waypoint> Waypoints = new ObservableCollection<Waypoint>();

        public ActionInfo ActionInfo = new ActionInfo();

        public FilterInfo FilterInfo = new FilterInfo();

        public WeaponAbility WeaponInfo = new WeaponAbility();

        public RestingInfo RestingInfo = new RestingInfo();

        public MiscSettings MiscSettings = new MiscSettings();
    }
}