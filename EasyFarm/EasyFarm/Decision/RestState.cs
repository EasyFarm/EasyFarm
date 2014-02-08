using EasyFarm.Engine;

class RestState : BaseState
{
    public RestState(ref GameEngine gameEngine) : base(ref gameEngine) { }

    public override bool CheckState()
    {
        return gameEngine.Player.shouldRest && gameEngine.IsWorking;
    }

    public override void EnterState()
    {

    }

    public override void RunState()
    {
        if (!gameEngine.Player.IsResting)
        {
            gameEngine.Player.RestingOn();
        }
    }

    public override void ExitState()
    {

    }
}
