
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
    public class CommandMonitor : BaseMonitor
    {
        public CommandMonitor(FFACE fface) : base(fface) { }

        protected override void CheckStatus(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (m_lock)
            {
                if (m_fface.Windower.ArgumentCount() > 0)
                {
                    for (short i = 0; i < m_fface.Windower.ArgumentCount(); i++)
                    {
                        var arg = m_fface.Windower.GetArgument(i);
                        OnChanged(new MonitorArgs<string>(arg));
                    }
                }
            }
        }
    }
}
