using System.Diagnostics;
using EasyFarm.PathingTools;
using EasyFarm.ProcessTools;
using EasyFarm.UnitTools;
using EasyFarm.PlayerTools;

namespace EasyFarm.Engine
{
    /// <summary>
    /// A class that acts as the knowledge source for our bot.
    /// It allows the bot to make informed decisions on what to do
    /// in its environment.
    /// </summary>
    public class GameState
    {
        public Process     Process     { get; set; }
        public FFInstance  FFInstance  { get; set; }
        public Pathing     Pathing     { get; set; }
        public Units       Units       { get; set; }
        public Player      Player      { get; set; }
        public Config      Config      { get; set; }
        public GameEngine  GameEngine  { get; set; }
        public Unit        Target      { get; set; }
        private GameState  m_gameState;

        public GameState()
        {
            Config = new Config();
        }

        public GameState(ref GameEngine GameEngine): this()
        {
            m_gameState      = this;
            this.Process     = GameEngine.Process;
            this.FFInstance  = GameEngine.FFInstance;
            this.Pathing     = new Pathing(ref m_gameState);
            this.Units       = new Units(ref m_gameState);
            this.Player      = new Player(ref m_gameState);
            this.GameEngine  = GameEngine;
            this.Target      = Unit.CreateUnit(0);
        }

        public bool IsResting
        {
            get
            {
                return Player.IsInjured() && !Player.IsRestingBlocked() && !Player.IsAggroed();
            }
        }

        public bool IsTraveling
        {
            get 
            {
                return Config.Waypoints.Length > 0 && Units.GetTarget().ID == 0 && !Player.IsInjured() && !Player.IsUnable();
            }
        }
    }
}
