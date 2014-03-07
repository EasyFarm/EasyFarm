
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
using EasyFarm.PlayerTools;
using System.Collections.ObjectModel;
using EasyFarm.UtilityTools;
using EasyFarm.MVVM;
using System.Xml.Serialization;
using MvvmFoundation.Wpf;
using EasyFarm.Views.Classes;
using EasyFarm.Classes;

namespace EasyFarm.Engine
{
    /// <summary>
    /// A configuration file for the user to edit through his GUI.
    /// Gives the bot access to allow of his decisions.
    /// </summary>
    public class Config : ObservableObject
    {
        /// <summary>
        /// Reference of the game engine
        /// </summary>
        private GameEngine m_gameEngine;
        private string m_statusBarText;

        private Config() { }

        public Config(ref GameEngine m_gameEngine)
        {            
            this.m_gameEngine = m_gameEngine;

            IgnoredList = new ObservableCollection<string>();
            TargetsList = new ObservableCollection<string>();
            Waypoints = new ObservableCollection<FFACE.Position>();
            Weaponskill = new WeaponAbility();

            HighHP = 0;
            LowHP = 0;
            WSHealthThreshold = 100;

            BattleAggro = true;
            BattlePartyClaimed = true;
            BattleUnclaimed = true;

            BattleList = new ObservableCollection<Ability>();
            StartList = new ObservableCollection<Ability>();
            EndList = new ObservableCollection<Ability>();
            HealingList = new ObservableCollection<ListItem<HealingAbility>>();

            StartListSelected = true;
            BattleListSelected = false;
            EndListSelected = false;
        }

        /// <summary>
        /// List of actions that should be used before battle
        /// </summary>
        public ObservableCollection<Ability> StartList { get; set; }
     
        /// <summary>
        /// List of actions taht should be used at the end of battle
        /// </summary>
        public ObservableCollection<Ability> EndList { get; set; }
        
        /// <summary>
        /// List of actions that should be used in battle
        /// </summary>
        public ObservableCollection<Ability> BattleList { get; set; }
        
        /// <summary>
        /// List of actions that should be used when injured
        /// </summary>
        public ObservableCollection<ListItem<HealingAbility>> HealingList { get; set; }
        
        /// <summary>
        /// List of mob's name of the mobs that should be ignored
        /// </summary>
        public ObservableCollection<String> IgnoredList { get; set; }
        
        /// <summary>
        /// List of mob's name of the mobs that should be killed
        /// </summary>
        public ObservableCollection<String> TargetsList { get; set; }
        
        /// <summary>
        /// List of all waypoints that make up the bots path
        /// </summary>
        public ObservableCollection<FFACE.Position> Waypoints { get; set; }
        
        /// <summary>
        /// The weaponskill that should be used when we reach 100% tp
        /// </summary>
        public WeaponAbility Weaponskill { get; set; }
        
        /// <summary>
        /// Tells us when to use the weaponskill when the mob's hp reaches this level
        /// </summary>
        public int WSHealthThreshold { get; set; }
        
        /// <summary>
        /// Should we kill aggroing creatures?
        /// </summary>
        public bool BattleAggro { get; set; }
        
        /// <summary>
        /// Should we kill party members creatures?
        /// </summary>
        public bool BattlePartyClaimed { get; set; }
        
        /// <summary>
        /// Should we kill unclaimed creatures?
        /// </summary>
        public bool BattleUnclaimed { get; set; }
        
        /// <summary>
        /// The name of the mob that will be added to the TargetsList
        /// </summary>
        public string TargetsName { get; set; }
        
        /// <summary>
        /// Name of the mob that should be added to the IgnoreList
        /// </summary>
        public string IgnoredName { get; set; }
        
        /// <summary>
        /// The text dislayed at the bottom of the screen
        /// </summary>
        [XmlIgnore]
        public string StatusBarText 
        {
            get { return m_statusBarText; }
            set 
            {
                this.m_statusBarText = value;
                RaisePropertyChanged("StatusBarText");
            }
        }
        
        /// <summary>
        /// Is the BattleList Selected in the battle tab?
        /// </summary>
        public bool BattleListSelected { get; set; }
        
        /// <summary>
        /// Is the StartList selected in the battle tab?
        /// </summary>
        public bool StartListSelected { get; set; }
        
        /// <summary>
        /// Is the End list selected in the battle tab?
        /// </summary>
        public bool EndListSelected { get; set; }
        
        /// <summary>
        /// The name of the ability going to be added to either the Battle/Start/End Action Lists
        /// </summary>
        public string BattleActionName { get; set; }
        
        /// <summary>
        /// The name of the weaponskill that will be created
        /// </summary>
        public string WSName { get; set; }
        
        /// <summary>
        /// The max distance the weaponskill should be used at
        /// </summary>
        public double WSDistance { get; set; }
        
        /// <summary>
        /// The value we should rest mp at
        /// </summary>
        public int LowMP { get; set; }
        
        /// <summary>
        /// The value we can get up from resting mp
        /// </summary>
        public int HighMP { get; set; }
        
        /// <summary>
        /// The value we can get up from resting hp
        /// </summary>
        public int HighHP { get; set; }
        
        /// <summary>
        /// The value we should rest hp at.
        /// </summary>
        public int LowHP { get; set; }
        
        /// <summary>
        /// Should we rest hp at all?
        /// </summary>
        public bool IsRestingHPEnabled { get; set; }
        
        /// <summary>
        /// Should we rest mp at all?
        /// </summary>
        public bool IsRestingMPEnabled { get; set; }
    }
}