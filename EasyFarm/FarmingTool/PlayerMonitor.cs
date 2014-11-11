
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
using System.Timers;
using ZeroLimits.XITool;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.FarmingTool
{
    public class PlayerMonitor : BaseMonitor
    {
        private UnitService m_unitService;
        private bool m_detected = false;

        public PlayerMonitor(FFACE fface) : base(fface)
        {
            this.m_unitService = new UnitService(fface);
        }

        protected override void CheckStatus(object sender, ElapsedEventArgs e)
        {
            lock (m_lock)
            {
                bool detected = m_unitService.GetTarget(UnitFilters.PCFilter(m_fface)) != null;

                if (m_detected != detected)
                {
                    OnChanged(new MonitorArgs<bool>(detected));
                    m_detected = detected;
                }
            }
        }

        public bool Detected 
        {
            get { return this.m_detected; }
        }        
    }
}