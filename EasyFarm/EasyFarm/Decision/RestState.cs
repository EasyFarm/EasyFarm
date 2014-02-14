using EasyFarm.Engine;

class RestState : BaseState
{
    public RestState(ref GameEngine gameEngine) : base(ref gameEngine) { }

    public override bool CheckState()
    {
        return gameEngine.PlayerData.shouldRest && gameEngine.IsWorking;
    }

    public override void EnterState()
    {

    }

    public override void RunState()
    {
        if (!gameEngine.PlayerData.IsResting)
        {
            gameEngine.Resting.On();
        }
    }

    public override void ExitState()
    {

    }
}
