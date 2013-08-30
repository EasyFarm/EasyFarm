using System;
using EasyFarm.PlayerTools;

public abstract class BaseState : IComparable<BaseState>
{
    public bool Enabled;
    public int Priority;
    public abstract bool CheckState();
    public abstract void EnterState();
    public abstract void RunState();
    public abstract void ExitState();

    public int CompareTo(BaseState other)
    {
        return -this.Priority.CompareTo(other.Priority);
    }
}