
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

﻿using EasyFarm.Classes;
using EasyFarm.UtilityTools;
using FFACETools;
using System;
using System.Collections.Generic;
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
        public Process Process;

        /// <summary>
        /// The FFACE and Process Instance of the current FFXI Instance
        /// </summary>
        public FFInstance Session;

        /// <summary>
        /// The decision making machinery for the bot
        /// </summary>
        public FiniteStateEngine StateMachine;

        /// <summary>
        /// Contains the user's preferences that he makes through 
        /// his user interface.
        /// </summary>
        public Config UserSettings = new Config();

        /// <summary>
        /// Contains data on Creatures/NPCS in the environment. 
        /// Does not contain player information.
        /// </summary>
        public UnitService Units;

        /// <summary>
        /// Contains method for combat and player status info.
        /// </summary>
        public CombatService CombatService;

        /// <summary>
        /// Some base information about the player like if they are injured or fighting.
        /// </summary>
        public PlayerData PlayerData;

        /// <summary>
        /// Details about our current target.
        /// </summary>
        public TargetData TargetData;

        /// <summary>
        /// Functions and details for resting.
        /// </summary>
        public RestingService RestingService;

        /// <summary>
        /// Contains the methods needed for executing actions
        /// </summary>
        public AbilityExecutor AbilityExecutor;

        /// <summary>
        /// Contains all of the player's potential moves.
        /// </summary>
        public PlayerActions PlayerActions;

        /// <summary>
        /// Retrieves ability objects.
        /// </summary>
        public AbilityService AbilityService = new AbilityService();

        /// <summary>
        /// Determines with an action can be used.
        /// </summary>
        public ActionBlocked ActionBlocked;

        /// <summary>
        /// Tells us whether the bot is working or not.
        /// </summary>
        public bool IsWorking = false;
        
        /// <summary>
        /// Our game engine singleton instance
        /// </summary>
        private static GameEngine _engine;
        #endregion

        #region Constructors
        private GameEngine(Process process)
        {
            SetProcess(process);
        }

        private GameEngine()
        {
                
        }
        #endregion

        #region Methods
        /// <summary>
        /// Assumes the user will supply a process object later on.
        /// </summary>
        /// <returns></returns>
        public static GameEngine GetInstance()
        {
            return _engine ?? new GameEngine();
        }

        /// <summary>
        /// Returns/Creates a game engine object for the supplied process
        /// </summary>
        /// <returns></returns>
        public static GameEngine GetInstance(Process process)
        {
            if (_engine == null)
            {
                return new GameEngine(process);
            }
            else if (_engine.Process != process)
            {
                _engine.SetProcess(process);
                return _engine;
            }
            else 
            {
                return _engine;
            }
        }

        /// <summary>
        /// Sets up the game engine for a given process object.
        /// </summary>
        /// <param name="process"></param>
        public void SetProcess(Process process)
        {
            _engine = this;
            Process = process;
            Session = new FFInstance(this.Process);
            StateMachine = new FiniteStateEngine(ref _engine);
            Units = UnitService.GetInstance(this.Session.Instance);
            CombatService = new CombatService(ref _engine);
            PlayerData = new PlayerData(ref _engine);
            TargetData = new TargetData(ref _engine);
            RestingService = RestingService.GetInstance(this.Session.Instance);
            AbilityExecutor = new AbilityExecutor(ref _engine);
            PlayerActions = new PlayerActions(ref _engine);
            ActionBlocked = new ActionBlocked(ref _engine);
            AbilityService = new AbilityService();
        }

        /// <summary>
        /// Start the bot up
        /// </summary>
        public void Start()
        {
            StateMachine.Start();
            IsWorking = true;
        }

        /// <summary>
        /// Stop the bot from going any further
        /// </summary>
        public void Stop()
        {
            StateMachine.Stop();
            IsWorking = false;
        }

        /// <summary>
        /// Saves the settings of Config object to file for later retrieval.
        /// </summary>
        /// <param name="Engine"></param>
        public void SaveSettings(GameEngine Engine)
        {
            UserSettings.FilterInfo = UnitService.GetInstance().FilterInfo;
            String Filename = Session.Instance.Player.Name + "_UserPref.xml";
            Utilities.Serialize(Filename, UserSettings);
        }

        /// <summary>
        /// Loads the settings from the player specific configuration file to the Config obj.
        /// </summary>
        public void LoadSettings()
        {
            String Filename = Session.Instance.Player.Name + "_UserPref.xml";
            UserSettings = Utilities.Deserialize(Filename, UserSettings);
            UnitService.GetInstance().FilterInfo = UserSettings.FilterInfo;
        }
        #endregion
    }
}
