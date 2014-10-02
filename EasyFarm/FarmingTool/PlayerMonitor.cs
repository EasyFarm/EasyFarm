using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ZeroLimits.XITools;

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