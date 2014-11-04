using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroLimits.XITool;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.FarmingTool
{
    public class StuckMonitor : BaseMonitor
    {
        private MovingUnit Player { get; set; }

        private int ID { get; set; }

        public StuckMonitor(FFACE fface) : base(fface) 
        {
            if (Unit.Session == null) Unit.Session = fface;
            this.ID = fface.Player.ID;
            this.Player = new MovingUnit(this.ID);                       
        }

        protected override void CheckStatus(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (this.m_lock)
            {
                if (this.Player.IsStuck)
                {
                    this.OnChanged(new MonitorArgs<bool>(this.Player.IsStuck));
                }
            }
        }
    }
}
