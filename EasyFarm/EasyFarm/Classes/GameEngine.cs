using EasyFarm.Classes;
using EasyFarm.PathingTools;
using EasyFarm.PlayerTools;
using EasyFarm.ProcessTools;
using EasyFarm.UnitTools;
using EasyFarm.UtilityTools;
using System;
using System.Diagnostics;

namespace EasyFarm.Engine
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
        /// A collection of data used by the bot to make decisions
        /// It doesn't use this anymore...
        /// </summary>
        private GameState m_gameState = null;
        
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
        private Units m_units = null;

        /// <summary>
        /// Contains method for combat and player status info.
        /// </summary>
        private Combat m_combat = null;

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
        private Resting m_resting = null;

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
            // m_gameState = new GameState(ref m_gameEngine);
            m_stateMachine = new FiniteStateEngine(ref m_gameEngine);
            m_config = new Config(ref m_gameEngine);
            m_pathing = new Pathing(ref m_gameEngine);
            m_units = new Units(ref m_gameEngine);
            m_combat = new Combat(ref m_gameEngine);
            m_target = Unit.CreateUnit(0);
            m_playerData = new PlayerData(ref m_gameEngine);
            m_targetData = new TargetData(ref m_gameEngine);
            m_resting = new Resting(ref m_gameEngine);
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
        /// Originally the bots knowledge source; has been replaced by the engine obj.
        /// </summary>
        public GameState GameState
        {
            get { return m_gameState; }
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
        public Units Units
        {
            get { return m_units; }
        }

        /// <summary>
        /// Returns a player object; contains mostly combat methods and player status information.
        /// </summary>
        public Combat Combat
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
        public Resting Resting
        { 
            get { return m_resting; } 
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
