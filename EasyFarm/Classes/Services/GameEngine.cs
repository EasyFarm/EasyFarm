
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
        private Process _process = null;

        /// <summary>
        /// The FFACE and Process Instance of the current FFXI Instance
        /// </summary>
        private Session _ffinstance = null;      
        
        /// <summary>
        /// The engine to run the bot. Contains all the information the bot 
        /// needs to perform its job.
        /// </summary>
        private GameEngine _engine = null;
        
        /// <summary>
        /// The decision making machinery for the bot
        /// </summary>
        private FiniteStateEngine _statemachine = null;

        /// <summary>
        /// Contains the user's preferences that he makes through 
        /// his user interface.
        /// </summary>
        private Config _config = null;
        
        /// <summary>
        /// Contains the code for moving the bot along a path.
        /// </summary>
        // private Pathing m_pathing = null;
        
        /// <summary>
        /// Contains data on Creatures/NPCS in the environment. 
        /// Does not contain player information.
        /// </summary>
        private UnitService _units = null;

        /// <summary>
        /// Contains method for combat and player status info.
        /// </summary>
        private CombatService _combat = null;

        /// <summary>
        /// The current target the bot is trying to kill.
        /// </summary>
        private Unit _target = null;

        /// <summary>
        /// Some base information about the player like if they are injured or fighting.
        /// </summary>
        private PlayerData _playerData = null;

        /// <summary>
        /// Details about our current target.
        /// </summary>
        private TargetData _targetData = null;

        /// <summary>
        /// Functions and details for resting.
        /// </summary>
        private RestingService _resting = null;
        
        /// <summary>
        /// Contains the methods needed for executing actions
        /// </summary>
        private AbilityExecutor _abilityExecutor;
        
        /// <summary>
        /// Contains all of the player's potential moves.
        /// </summary>
        private PlayerActions _playerActions;
        
        /// <summary>
        /// Retrieves ability objects.
        /// </summary>
        private AbilityService _abilityService;
        
        /// <summary>
        /// Determines with an action can be used.
        /// </summary>
        private ActionBlocked _actionBlocked;

        #endregion

        #region Constructors
        private GameEngine()
        {
            _engine = this;
            IsWorking = false;
        }

        public GameEngine(Process process)
            : this()
        {
            _process = process;
            _ffinstance = new Session(this._process);
            _statemachine = new FiniteStateEngine(ref _engine);
            _config = new Config();
            //m_pathing = new Pathing(ref m_gameEngine);
            _units = new UnitService(ref _engine);
            _combat = new CombatService(ref _engine);
            _target = Unit.CreateUnit(0);
            _playerData = new PlayerData(ref _engine);
            _targetData = new TargetData(ref _engine);
            _resting = new RestingService(ref _engine);
            _abilityExecutor = new AbilityExecutor(ref _engine);
            _playerActions = new PlayerActions(ref _engine);
            _actionBlocked = new ActionBlocked(ref _engine);
            _abilityService = new AbilityService();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Contains process information
        /// </summary>
        public Process Process
        {
            get { return _process; }
        }

        /// <summary>
        /// Contains process and FFACE Methods and information
        /// </summary>
        public Session FFInstance
        {
            get { return _ffinstance; }
        }
        
        /// <summary>
        /// The engine that makes decisions for the bot and takes actions.
        /// </summary>
        public FiniteStateEngine FiniteStateEngine
        {
            get { return _statemachine; }
        }

        /// <summary>
        /// A file that contains all of the settings set by the user through his/her user interface.
        /// </summary>
        public Config Config
        {
            get { return _config; }
        }
        
        /// <summary>
        /// Contains the methods used for moving the bot around
        /// </summary>
        //public Pathing Pathing
        //{
        //    get { return m_pathing; }
        //}

        /// <summary>
        /// Contains the mob array excluding surrounding players.
        /// </summary>
        public UnitService Units
        {
            get { return _units; }
        }

        /// <summary>
        /// Returns a player object; contains mostly combat methods and player status information.
        /// </summary>
        public CombatService Combat
        {
            get { return _combat; }
        }

        /// <summary>
        /// Returns player specific details. 
        /// </summary>
        public PlayerData PlayerData
        {
            get { return _playerData; }
        }

        /// <summary>
        /// Returns target specific details
        /// </summary>
        public TargetData TargetData
        {
            get { return _targetData; }
        }

        /// <summary>
        /// Contains functions and information for resting.
        /// </summary>
        public RestingService Resting
        { 
            get { return _resting; } 
        }

        public PlayerActions PlayerActions 
        {
            get { return _playerActions; }
        }

        public AbilityExecutor AbilityExecutor 
        {
            get { return _abilityExecutor; }
        }

        public AbilityService AbilityService 
        {
            get { return _abilityService; }
        }

        public ActionBlocked ActionBlocked 
        {
            get { return _actionBlocked; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Start the bot up
        /// </summary>
        public void Start()
        {
            _statemachine.Start();
            IsWorking = true;
        }

        /// <summary>
        /// Stop the bot from going any further
        /// </summary>
        public void Stop()
        {
            _statemachine.Stop();
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
            _config = Utilities.Deserialize(Filename, Config);
        }
        #endregion
    }
}
