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
            return gameEngine.PlayerData.shouldFight;
        }

        public override void EnterState()
        {
            gameEngine.Combat.Enter();
            gameEngine.Resting.Off();
        }

        public override void RunState()
        {
            gameEngine.Combat.Battle();
        }

        public override void ExitState()
        {
            gameEngine.Combat.Exit();
        }
    }
}
