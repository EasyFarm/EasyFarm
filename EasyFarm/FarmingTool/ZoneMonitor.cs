using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ZeroLimits.XITool;

namespace EasyFarm.FarmingTool
{
    public class ZoneMonitor : BaseMonitor
    {
        private Zone m_zone;

        public ZoneMonitor(FFACE fface) : base(fface) { }

        protected override void CheckStatus(object sender, ElapsedEventArgs e)
        {
            lock (m_lock)
            {
                Zone zone = m_fface.Player.Zone;

                if (m_zone != zone)
                {
                    OnChanged(new MonitorArgs<Zone>(zone));
                    m_zone = zone;
                }
            }
        }

        public Zone Zone
        {
            get { return this.m_zone; }
        }        
    }
}