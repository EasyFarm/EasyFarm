using EasyFarm.Engine;
using EasyFarm.PlayerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.FSM
{
    class HealingState : BaseState
    {
        public HealingState(ref GameEngine gameEngine) : base(ref gameEngine) { }

        public override bool CheckState()
        {
            return gameEngine.Player.shouldHeal && !gameEngine.Player.shouldRest;
        }

        public override void EnterState()
        {
            gameEngine.Player.RestingOff();
        }

        public override void RunState()
        {
            // Use an ability to heal from the healing list if we can
            if(gameEngine.Player.HealingList.Count > 0)
            {
                // Check for actions available
                var act = gameEngine.Player.HealingList.FirstOrDefault();
                if (act == null) { return; }
                //
                else { gameEngine.Player.UseAbility(act); }
            }
        }

        public override void ExitState() { }
    }
}
