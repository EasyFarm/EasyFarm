using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyFarm.Engine;

namespace EasyFarm.FSM
{
    class IdleState : BaseState
    {
        public IdleState(ref GameState GameState) : base(ref GameState) { }

        public override bool CheckState()
        {
            bool IsInjured = GameState.Player.IsInjured();
            bool IsRestingBlocked = GameState.Player.IsRestingBlocked();

            return IsInjured && IsRestingBlocked;
        }

        public override void EnterState()
        {
            // Do nothing
        }

        public override void RunState()
        {
            // Do nothing
        }

        public override void ExitState()
        {
            // Do nothing
        }
    }
}
