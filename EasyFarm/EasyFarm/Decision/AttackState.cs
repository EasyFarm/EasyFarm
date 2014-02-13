using EasyFarm.Engine;

namespace EasyFarm.FSM
{
    /// <summary>
    /// A class for defeating monsters.
    /// </summary>
    class AttackState : BaseState
    {
        public AttackState(ref GameEngine gameEngine) : base(ref gameEngine) { }

        public override bool CheckState()
        {
            return gameEngine.Player.shouldFight;
        }

        public override void EnterState()
        {
            gameEngine.Player.Enter();
            gameEngine.Player.RestingOff();
        }

        public override void RunState()
        {
            gameEngine.Player.Battle();
        }

        public override void ExitState()
        {
            gameEngine.Player.Exit();
        }
    }
}
