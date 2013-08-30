using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyFarm.Engine;

class RestState : BaseState
{
    public RestState(ref GameState GameState) : base(ref GameState) { }

    public override bool CheckState()
    {        
        return GameState.IsResting;
    }

    public override void EnterState()
    {
        
    }

    public override void RunState()
    {
        GameState.Player.RestingOn();
    }

    public override void ExitState()
    {
        GameState.Player.RestingOff();
    }
}
