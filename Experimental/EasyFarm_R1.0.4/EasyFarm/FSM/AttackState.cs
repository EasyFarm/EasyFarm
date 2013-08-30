using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyFarm.Engine;

namespace EasyFarm.FSM
{
    class AttackState : BaseState
    {
        public AttackState(ref GameState GameState) : base(ref GameState) { }

        public override bool CheckState()
        {
            GameState.Player.TargetUnit = GameState.Units.GetTarget();

            bool IsAggroed = GameState.Player.IsAggroed();
            bool IsMobAvailable = GameState.Player.TargetUnit.ID != 0;
            bool IsHealthy = !GameState.Player.IsInjured();
            bool IsUnitBattleReady = GameState.Player.IsUnitBattleReady();

            return IsUnitBattleReady && (IsAggroed || IsMobAvailable && IsHealthy);
        }

        public override void EnterState()
        {
            if (GameState.Player.IsResting())
            {
                GameState.Player.RestingOff();
            }

            if (GameState.FFInstance.Instance.Navigator.IsRunning())
            {
                GameState.FFInstance.Instance.Navigator.Reset();
            }

            GameState.Player.Enter();
        }

        public override void RunState()
        {
            GameState.Player.Battle();
        }

        public override void ExitState()
        {
            GameState.Player.Exit();
        }
    }
}
