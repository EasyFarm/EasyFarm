using System.Threading;
using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.States
{
    public class ZoneState : BaseState
    {
        private Zone _zone;

        public ZoneState(IMemoryAPI fface) : base(fface)
        {
            _zone = fface.Player.Zone;
        }

        public override bool Check()
        {
            var zone = fface.Player.Zone;
            return _zone != zone || fface.Player.Stats.Str == 0;
        }

        public override void Run()
        {
            // Set new zone.
            _zone = fface.Player.Zone;

            // Stop program from running to next waypoint.
            fface.Navigator.Reset();

            // Wait until we are done zoning.
            while (fface.Player.Stats.Str == 0)
            {
                Thread.Sleep(500);
            }
        }
    }
}
