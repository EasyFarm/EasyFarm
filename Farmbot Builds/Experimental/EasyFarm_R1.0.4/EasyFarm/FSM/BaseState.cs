using System;
using EasyFarm.Engine;
using System.Threading.Tasks;
using System.Threading;

public abstract class BaseState : IComparable<BaseState>
{
    public bool Enabled;
    public int Priority;
    public GameState GameState;
    
    public abstract bool CheckState();
    public abstract void EnterState();
    public abstract void RunState();
    public abstract void ExitState();

    public BaseState(ref GameState GameState)
    {
        this.GameState = GameState;
    }

    public int CompareTo(BaseState other)
    {
        return -this.Priority.CompareTo(other.Priority);
    }
}