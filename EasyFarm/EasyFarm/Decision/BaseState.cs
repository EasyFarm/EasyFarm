using System;
using EasyFarm.Engine;

public abstract class BaseState : IComparable<BaseState>
{
    public bool Enabled;
    public int Priority;
    protected GameEngine gameEngine;
    
    public abstract bool CheckState();
    public abstract void EnterState();
    public abstract void RunState();
    public abstract void ExitState();

    public BaseState(ref GameEngine gameEngine)
    {
        this.gameEngine = gameEngine;
    }

    public int CompareTo(BaseState other)
    {
        return -this.Priority.CompareTo(other.Priority);
    }
}