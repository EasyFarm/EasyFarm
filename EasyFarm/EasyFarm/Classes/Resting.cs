using EasyFarm.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    public class Resting
    {
        /// <summary>
        /// Command for resting
        /// </summary>
        const string RESTING_ON = "/heal on";

        /// <summary>
        /// Command for stopping resting
        /// </summary>
        const string RESTING_OFF = "/heal off";

        public Resting(ref GameEngine Engine)
        {
            this.Engine = Engine;
            this.PlayerData = Engine.PlayerData;
        }

        /// <summary>
        /// Makes the character stop resting
        /// </summary>
        public void Off()
        {
            if (PlayerData.IsResting) { Engine.FFInstance.Instance.Windower.SendString(RESTING_OFF); }
        }

        /// <summary>
        /// Makes the character rest
        /// </summary>
        public void On()
        {
            if (!PlayerData.IsResting) { Engine.FFInstance.Instance.Windower.SendString(RESTING_ON); }
        }

        /// <summary>
        /// All information about the bot.
        /// </summary>
        private GameEngine Engine { get; set; }

        /// <summary>
        /// Details about the player
        /// </summary>
        private PlayerData PlayerData { get; set; }
    }
}
