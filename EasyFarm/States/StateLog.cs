namespace EasyFarm.States
{
    public class StateLog
    {
        public StateLog(IState state, StateStatus status)
        {
            State = state;
            Status = status;
        }

        public StateStatus Status { get; }
        public IState State { get; }
    }
}