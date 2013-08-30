using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyFarm.PlayerTools;

namespace EasyFarm.FSM
{
    class InjuredState : BaseState
    {
        private Player Player;

        public InjuredState(Player Player)
        {
            this.Player = Player;
        }

        public override bool CheckState()
        {
            return this.Player.IsInjured();
        }

        public override void EnterState()
        {
            // Do nothing
        }

        public override void RunState()
        {
            Player.Heal();
        }

        public override void ExitState()
        {
            Player.StopHealing();
        }
    }
}
