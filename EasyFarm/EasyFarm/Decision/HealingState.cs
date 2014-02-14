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
            return gameEngine.PlayerData.shouldHeal && !gameEngine.PlayerData.shouldRest;
        }

        public override void EnterState()
        {
            gameEngine.Resting.Off();
        }

        public override void RunState()
        {
            // Use an ability to heal from the healing list if we can
            if(gameEngine.Combat.HealingList.Count > 0)
            {
                // Check for actions available
                var act = gameEngine.Combat.HealingList.FirstOrDefault();
                if (act == null) { return; }
                //
                else { gameEngine.Combat.UseAbility(act); }
            }
        }

        public override void ExitState() { }
    }
}
