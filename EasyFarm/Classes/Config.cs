
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
using MvvmFoundation.Wpf;
using EasyFarm.MVVM;
using EasyFarm.Classes;

namespace EasyFarm.Classes
{
    /// <summary>
    /// A configuration file for the user to edit through his GUI.
    /// Gives the bot access to allow of his decisions.
    /// </summary>
    public class Config : ObservableObject
    {
        private string m_statusBarText;
        private GameEngine GameEngine;
       
        public Config(ref GameEngine Engine)
        {
            this.GameEngine = Engine;
            SetDefaults();
        }

        public void SetDefaults()
        {
            FilterInfo = new FilterInfo();
            WeaponInfo = new WeaponInfo();
            RestingInfo = new RestingInfo();
            ActionInfo = new ActionInfo();
            Waypoints = new ObservableCollection<FFACE.Position>();
        }        
        
        /// <summary>
        /// List of all waypoints that make up the bots path
        /// </summary>
        public ObservableCollection<FFACE.Position> Waypoints { get; set; }                      
        
        /// <summary>
        /// The text dislayed at the bottom of the screen
        /// </summary>
        [XmlIgnore]
        public string StatusBarText 
        {
            get 
            { 
                return m_statusBarText; 
            }
            
            set 
            {
                this.m_statusBarText = value;
                RaisePropertyChanged("StatusBarText");
            }
        }

        public ActionInfo ActionInfo { get; set; }

        public FilterInfo FilterInfo { get; set; }

        public WeaponInfo WeaponInfo { get; set; }

        public RestingInfo RestingInfo { get; set; }
    }
}