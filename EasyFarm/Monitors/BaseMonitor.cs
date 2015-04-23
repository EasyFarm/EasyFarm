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

using System;
using System.Timers;
using FFACETools;

namespace EasyFarm.FarmingTool
{
    public delegate void StatusChanged(object sender, EventArgs e);

    public abstract class BaseMonitor : IDisposable
    {
        protected FFACE _fface;
        protected object _lock = new object();
        protected Timer m_timer = new Timer();

        protected BaseMonitor(FFACE fface)
            : this()
        {
            _fface = fface;
        }

        protected BaseMonitor()
        {
            m_timer.Elapsed += CheckStatus;
            m_timer.AutoReset = true;
            m_timer.Interval = 30;
        }

        public bool Enabled
        {
            get { return m_timer.Enabled; }
            set { m_timer.Enabled = value; }
        }

        public void Dispose()
        {
            m_timer.Dispose();
        }

        public event StatusChanged Changed;

        protected virtual void OnChanged(EventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }

        protected abstract void CheckStatus(object sender, ElapsedEventArgs e);

        public void Start()
        {
            m_timer.Start();
        }

        public void Stop()
        {
            m_timer.Stop();
        }
    }

    public class MonitorArgs<T> : EventArgs
    {
        public MonitorArgs(T status)
        {
            Status = status;
        }

        public T Status { get; set; }
    }
}