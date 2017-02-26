using System;
using System.Threading;
using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.States
{
    public class ZoneState : BaseState
    {
        public Zone Zone;

        public Action ZoningAction { get; set; } = () => TimeWaiter.Pause(500);

        public ZoneState(IMemoryAPI fface) : base(fface)
        {
            Zone = fface.Player.Zone;
        }

        public override bool Check()
        {
            var zone = fface.Player.Zone;
            return ZoneChanged(zone) || IsZoning;
        }

        private bool ZoneChanged(Zone zone)
        {
            return Zone != zone;
        }

        private bool IsZoning => fface.Player.Stats.Str == 0;

        public override void Run()
        {
            // Set new zone.
            Zone = fface.Player.Zone;

            // Stop program from running to next waypoint.
            fface.Navigator.Reset();

            // Wait until we are done zoning.
            while (IsZoning)
            {
                ZoningAction();
            }
        }
    }
}
