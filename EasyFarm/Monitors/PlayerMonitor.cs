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

using System.Linq;
using System.Timers;
using EasyFarm.Classes;
using FFACETools;

namespace EasyFarm.Monitors
{
    public class PlayerMonitor : BaseMonitor
    {
        private readonly UnitService _units;

        public PlayerMonitor(FFACE fface)
            : base(fface)
        {
            Detected = false;
            _units = new UnitService(fface);
        }

        public bool Detected { get; private set; }

        protected override void CheckStatus(object sender, ElapsedEventArgs e)
        {
            lock (Lock)
            {
                var detected = _units.PcArray
                    .Any(x => UnitFilters.PcFilter(FFACE, x));

                if (Detected != detected)
                {
                    OnChanged(new MonitorArgs<bool>(detected));
                    Detected = detected;
                }
            }
        }
    }
}