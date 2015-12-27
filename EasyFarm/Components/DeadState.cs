using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.Components
{
    public class DeadState : BaseState
    {
        public DeadState(MemoryWrapper fface) : base(fface) { }

        public override bool CheckComponent()
        {
            var status = fface.Player.Status;
            return status == Status.Dead1 || status == Status.Dead2;
        }

        public override void RunComponent()
        {
            // Stop program from running to next waypoint.
            fface.Navigator.Reset();

            // Stop the engine from running.
            EventPublisher.SendPauseEvent();
        }
    }
}
