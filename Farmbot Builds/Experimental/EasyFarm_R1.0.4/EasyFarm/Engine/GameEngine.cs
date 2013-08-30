using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using EasyFarm.PlayerTools;
using EasyFarm.ProcessTools;
using System.Threading.Tasks;

namespace EasyFarm.Engine
{
    public class GameEngine
    {
        #region Members
        Process m_process = null;
        FFInstance m_ffinstance = null;
        GameState m_gameState = null;
        GameEngine m_gameEngine = null;
        FiniteStateEngine m_stateMachine = null;
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
            m_stateMachine = new FiniteStateEngine(ref m_gameState);
        }
        #endregion

        #region Properties
        public Process Process
        {
            get { return m_process; }
            set { m_process = value; }
        }

        public FFInstance FFInstance
        {
            get { return m_ffinstance; }
            set { m_ffinstance = value; }
        }

        public GameState GameState
        {
            get { return m_gameState; }
            set { m_gameState = value; }
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
        #endregion
    }
}
