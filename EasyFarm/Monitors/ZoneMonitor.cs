/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

using System.Timers;
using FFACETools;

namespace EasyFarm.FarmingTool
{
    public class ZoneMonitor : BaseMonitor
    {
        public ZoneMonitor(FFACE fface) : base(fface)
        {
        }

        public Zone Zone { get; private set; }

        protected override void CheckStatus(object sender, ElapsedEventArgs e)
        {
            lock (_lock)
            {
                var zone = _fface.Player.Zone;

                if (Zone != zone || _fface.Player.Stats.Str == 0)
                {
                    OnChanged(new MonitorArgs<Zone>(zone));
                    Zone = zone;
                }
            }
        }
    }
}