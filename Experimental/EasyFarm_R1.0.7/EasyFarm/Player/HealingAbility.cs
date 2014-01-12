using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.PlayerTools
{
    /// <summary>
    /// Class for Healing Abilities
    /// </summary>
    public class HealingAbility
    {
        /// <summary>
        /// Can we use this abilitiy?
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// What is its name?
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The level to which we should use the ability
        /// </summary>
        public int TriggerLevel { get; set; }
    }
}
