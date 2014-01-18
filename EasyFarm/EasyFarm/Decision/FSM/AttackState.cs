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
            // Am I already fighting?
            bool IsFighting = gameEngine.Player.IsFighting;
            // Am I injured?
            bool IsInjured = gameEngine.Player.IsInjured;
            // Do I have aggro?
            bool hasAggro = gameEngine.Player.IsAggroed;
            // Am I current engaged?
            bool IsEngaged = gameEngine.Player.IsEngaged;
            // Can we battle the unit?
            bool IsAttackable = gameEngine.Player.IsUnitBattleReady;
            // Should we attack?
            bool IsAttacking = (IsAttackable && (IsFighting || hasAggro || (!IsFighting && !gameEngine.Player.shouldRest)));

            return IsAttacking && gameEngine.IsWorking;
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
