using EasyFarm.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Decision.FSM
{
    /// <summary>
    /// A state to pause the bot if it is dead.
    /// </summary>
    class DeadState : BaseState
    {
        public DeadState(ref GameEngine gameEngine) : base(ref gameEngine) { }

        public override bool CheckState() { return gameEngine.PlayerData.IsDead; }

        public override void EnterState() { }

        public override void RunState() { 
            gameEngine.Stop();
            gameEngine.Config.StatusBarText = "Stopped!";
        }

        public override void ExitState() { }
    }
}
