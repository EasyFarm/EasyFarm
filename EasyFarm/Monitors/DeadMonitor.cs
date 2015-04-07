
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

using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.FarmingTool
{
    public class DeadMonitor : BaseMonitor
    {
        public DeadMonitor(FFACE fface) : base(fface) { }

        protected override void CheckStatus(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (_lock)
            {
                if (_fface.Player.Status.Equals(Status.Dead1 | Status.Dead2))
                {
                    OnChanged(new MonitorArgs<Status>(_fface.Player.Status));
                }
            }
        }
    }
}
