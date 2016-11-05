using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.States
{
    public class DeadState : BaseState
    {
        public DeadState(IMemoryAPI fface) : base(fface) { }

        public override bool Check()
        {
            var status = fface.Player.Status;
            return status == Status.Dead1 || status == Status.Dead2;
        }

        public override void Run()
        {
            // Stop program from running to next waypoint.
            fface.Navigator.Reset();

            // Stop the engine from running.
            AppServices.SendPauseEvent();
        }
    }
}
