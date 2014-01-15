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
        public FFInstance  FFInstance  { get; set; }
        public Pathing     Pathing     { get; set; }
        public Units       Units       { get; set; }
        public Player      Player      { get; set; }
        public GameEngine  GameEngine  { get; set; }
        public Unit        Target      { get; set; }
        private GameState  m_gameState;

        public GameState(ref GameEngine GameEngine)
        {
            m_gameState      = this;
            this.FFInstance  = GameEngine.FFInstance;
            this.Pathing     = new Pathing(ref GameEngine);
            this.Units       = new Units(ref GameEngine);
            this.Player      = new Player(ref GameEngine);
            this.GameEngine  = GameEngine;
            this.Target      = Unit.CreateUnit(0);
        }
    }
}
