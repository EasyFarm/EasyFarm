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
            lock (m_lock)
            {
                if (m_fface.Player.Status.Equals(Status.Dead1 | Status.Dead2))
                {
                    OnChanged(new MonitorArgs<Status>(m_fface.Player.Status));
                }
            }
        }
    }
}
