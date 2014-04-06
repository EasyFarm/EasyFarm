
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

﻿using EasyFarm.Classes;
using EasyFarm.PathingTools;
using EasyFarm.UtilityTools;
using FFACETools;
using System;
using System.Diagnostics;

namespace EasyFarm.Classes
{
    /// <summary>
    /// An object that controls the bot and contains all the data. 
    /// The bot can access everything it needs from this object.
    /// </summary>
    public class GameEngine
    {
        #region Members
        /// <summary>
        /// The process of the current FFXI Instance
        /// </summary>
        private Process m_process = null;

        /// <summary>
        /// The FFACE and Process Instance of the current FFXI Instance
        /// </summary>
        private FFInstance m_ffinstance = null;      
        
        /// <summary>
        /// The engine to run the bot. Contains all the information the bot 
        /// needs to perform its job.
        /// </summary>
        private GameEngine m_gameEngine = null;
        
        /// <summary>
        /// The decision making machinery for the bot
        /// </summary>
        private FiniteStateEngine m_stateMachine = null;

        /// <summary>
        /// Contains the user's preferences that he makes through 
        /// his user interface.
        /// </summary>
        private Config m_config = null;
        
        /// <summary>
        /// Contains the code for moving the bot along a path.
        /// </summary>
        private Pathing m_pathing = null;
        
        /// <summary>
        /// Contains data on Creatures/NPCS in the environment. 
        /// Does not contain player information.
        /// </summary>
        private UnitService m_units = null;

        /// <summary>
        /// Contains method for combat and player status info.
        /// </summary>
        private CombatService m_combat = null;

        /// <summary>
        /// The current target the bot is trying to kill.
        /// </summary>
        private Unit m_target = null;

        /// <summary>
        /// Some base information about the player like if they are injured or fighting.
        /// </summary>
        private PlayerData m_playerData = null;

        /// <summary>
        /// Details about our current target.
        /// </summary>
        private TargetData m_targetData = null;

        /// <summary>
        /// Functions and details for resting.
        /// </summary>
        private RestingService m_resting = null;
        
        /// <summary>
        /// Contains the methods needed for executing actions
        /// </summary>
        private AbilityExecutor m_abilityExecutor;
        
        /// <summary>
        /// Contains all of the player's potential moves.
        /// </summary>
        private PlayerActions m_playerActions;
        
        /// <summary>
        /// Retrieves ability objects.
        /// </summary>
        private AbilityService m_abilityService;
        
        /// <summary>
        /// Determines with an action can be used.
        /// </summary>
        private ActionBlocked m_ActionBlocked;

        #endregion

        #region Constructors
        private GameEngine()
        {
            m_gameEngine = this;
            IsWorking = false;
        }

        public GameEngine(Process Process)
            : this()
        {
            m_process = Process;
            m_ffinstance = new FFInstance(this.m_process);
            m_stateMachine = new FiniteStateEngine(ref m_gameEngine);
            m_config = new Config();
            m_pathing = new Pathing(ref m_gameEngine);
            m_units = new UnitService(ref m_gameEngine);
            m_combat = new CombatService(ref m_gameEngine);
            m_target = Unit.CreateUnit(0);
            m_playerData = new PlayerData(ref m_gameEngine);
            m_targetData = new TargetData(ref m_gameEngine);
            m_resting = new RestingService(ref m_gameEngine);
            m_abilityExecutor = new AbilityExecutor(ref m_gameEngine);
            m_playerActions = new PlayerActions(ref m_gameEngine);
            m_ActionBlocked = new ActionBlocked(ref m_gameEngine);
            m_abilityService = new AbilityService();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Contains process information
        /// </summary>
        public Process Process
        {
            get { return m_process; }
        }

        /// <summary>
        /// Contains process and FFACE Methods and information
        /// </summary>
        public FFInstance FFInstance
        {
            get { return m_ffinstance; }
        }
        
        /// <summary>
        /// The engine that makes decisions for the bot and takes actions.
        /// </summary>
        public FiniteStateEngine FiniteStateEngine
        {
            get { return m_stateMachine; }
        }

        /// <summary>
        /// A file that contains all of the settings set by the user through his/her user interface.
        /// </summary>
        public Config Config
        {
            get { return m_config; }
        }
        
        /// <summary>
        /// Contains the methods used for moving the bot around
        /// </summary>
        public Pathing Pathing
        {
            get { return m_pathing; }
        }

        /// <summary>
        /// Contains the mob array excluding surrounding players.
        /// </summary>
        public UnitService Units
        {
            get { return m_units; }
        }

        /// <summary>
        /// Returns a player object; contains mostly combat methods and player status information.
        /// </summary>
        public CombatService Combat
        {
            get { return m_combat; }
        }

        /// <summary>
        /// Returns player specific details. 
        /// </summary>
        public PlayerData PlayerData
        {
            get { return m_playerData; }
        }

        /// <summary>
        /// Returns target specific details
        /// </summary>
        public TargetData TargetData
        {
            get { return m_targetData; }
        }

        /// <summary>
        /// Contains functions and information for resting.
        /// </summary>
        public RestingService Resting
        { 
            get { return m_resting; } 
        }

        public PlayerActions PlayerActions 
        {
            get { return m_playerActions; }
        }

        public AbilityExecutor AbilityExecutor 
        {
            get { return m_abilityExecutor; }
        }

        public AbilityService AbilityService 
        {
            get { return m_abilityService; }
        }

        public ActionBlocked ActionBlocked 
        {
            get { return m_ActionBlocked; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Start the bot up
        /// </summary>
        public void Start()
        {
            m_stateMachine.Start();
            IsWorking = true;
        }

        /// <summary>
        /// Stop the bot from going any further
        /// </summary>
        public void Stop()
        {
            m_stateMachine.Stop();
            IsWorking = false;
        }

        /// <summary>
        /// Is the bot working?
        /// </summary>
        public bool IsWorking { get; set; }

        /// <summary>
        /// Saves the settings of Config object to file for later retrieval.
        /// </summary>
        /// <param name="Engine"></param>
        public void SaveSettings(GameEngine Engine)
        {
            String Filename = FFInstance.Instance.Player.Name + "_UserPref.xml";
            Utilities.Serialize(Filename, Config);
        }

        /// <summary>
        /// Loads the settings from the player specific configuration file to the Config obj.
        /// </summary>
        public void LoadSettings()
        {
            String Filename = FFInstance.Instance.Player.Name + "_UserPref.xml";
            m_config = Utilities.Deserialize(Filename, Config);
        }
        #endregion
    }
}
