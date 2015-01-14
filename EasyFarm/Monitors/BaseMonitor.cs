
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

namespace EasyFarm.FarmingTool
{
    public delegate void StatusChanged(object sender, EventArgs e);

    public abstract class BaseMonitor : IDisposable
    {
        public event StatusChanged Changed;

        protected Timer m_timer = new Timer();
        protected Object m_lock = new Object();
        protected FFACE m_fface;

        protected BaseMonitor(FFACE fface)
            : this()
        {
            this.m_fface = fface;
        }

        protected BaseMonitor()
        {
            m_timer.Elapsed += CheckStatus;
            m_timer.AutoReset = true;
            m_timer.Interval = 30;
        }

        protected virtual void OnChanged(EventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }

        protected abstract void CheckStatus(object sender, ElapsedEventArgs e);

        public void Start() { this.m_timer.Start(); }

        public void Stop() { this.m_timer.Stop(); }

        public bool Enabled
        {
            get { return m_timer.Enabled; }
            set { m_timer.Enabled = value; }
        }

        public void Dispose()
        {
            this.m_timer.Dispose();
        }
    }

    public class MonitorArgs<T> : EventArgs
    {
        private T m_status;

        public MonitorArgs(T status)
        {
            this.m_status = status;
        }

        public T Status
        {
            get { return m_status; }
            set { this.m_status = value; }
        }
    }
}
