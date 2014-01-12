using EasyFarm.PathingTools;
using EasyFarm.PlayerTools;
using EasyFarm.ProcessTools;
using EasyFarm.UnitTools;
using EasyFarm.UtilityTools;
using System;
using System.Diagnostics;

namespace EasyFarm.Engine
{
    public class GameEngine
    {
        #region Members
        private Process m_process = null;
        private FFInstance m_ffinstance = null;
        private GameState m_gameState = null;
        private GameEngine m_gameEngine = null;
        private FiniteStateEngine m_stateMachine = null;
        private Config m_config = null;
        private Pathing m_pathing = null;
        private Units m_units = null;
        private Player m_player = null;
        private Unit m_target = null;
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
            m_gameState = new GameState(ref m_gameEngine);
            m_stateMachine = new FiniteStateEngine(ref m_gameEngine);
            m_config = new Config(ref m_gameEngine);
            m_pathing = new Pathing(ref m_gameEngine);
            m_units = new Units(ref m_gameEngine);
            m_player = new Player(ref m_gameEngine);
            m_target = Unit.CreateUnit(0);
        }
        #endregion

        #region Properties
        public Process Process
        {
            get { return m_process; }
        }

        public FFInstance FFInstance
        {
            get { return m_ffinstance; }
        }

        public GameState GameState
        {
            get { return m_gameState; }
        }

        public FiniteStateEngine FiniteStateEngine
        {
            get { return m_stateMachine; }
        }

        public Config Config
        {
            get { return m_config; }
        }

        public Pathing Pathing
        {
            get { return m_pathing; }
        }

        public Units Units
        {
            get { return m_units; }
        }

        public Player Player
        {
            get { return m_player; }
        }
        #endregion

        #region Methods
        public void Start()
        {
            m_stateMachine.Start();
            IsWorking = true;
        }

        public void Stop()
        {
            m_stateMachine.Stop();
            IsWorking = false;
        }

        public bool IsWorking { get; set; }

        public void SaveSettings(GameEngine Engine)
        {
            String Filename = Engine.FFInstance.Instance.Player.Name + "_UserPref.xml";
            Utilities.Serialize(Filename, Config);
        }

        public void LoadSettings()
        {
            String Filename = m_gameState.FFInstance.Instance.Player.Name + "_UserPref.xml";
            m_config = Utilities.Deserialize(Filename, Config);
        }
        #endregion
    }
}
