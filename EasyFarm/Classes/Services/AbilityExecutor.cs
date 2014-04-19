using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    /// <summary>
    /// A class for using abilties.
    /// </summary>

    public class AbilityExecutor
    {
        private GameEngine _engine;

        public AbilityExecutor(ref GameEngine gameEngine)
        {
            this._engine = gameEngine;
        }

        /// <summary>
        /// Performs a list of actions. 
        /// Could be spells or job abilities. 
        /// Does not validate actions.
        /// </summary>
        /// <param name="actions"></param>
        /// <param name="unit"></param>
        public void ExecuteActions(List<Ability> actions, Action callback)
        {
            foreach (var act in actions)
            {
                callback();
                UseAbility(act);
            }
        }

        /// <summary>
        /// Attempts to use the passed in ability
        /// </summary>
        /// <param name="Ability"></param>
        public void UseAbility(Ability Ability)
        {
            // Set the duration to spell time or 50 for an ability
            int SleepDuration = Ability.IsSpell ? (int)Ability.CastTime + 1500 : 50;

            // Sleep for a second to pause the bots motion
            System.Threading.Thread.Sleep(1000);

            // Send it to the game
            _engine.FFInstance.Instance.Windower.SendString(Ability.ToString());

            // Sleep for the cast duration
            System.Threading.Thread.Sleep(SleepDuration);
        }
    }
}
