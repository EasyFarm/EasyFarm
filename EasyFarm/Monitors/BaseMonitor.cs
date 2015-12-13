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

namespace EasyFarm.Monitors
{
    public delegate void StatusChanged(object sender, EventArgs e);

    public abstract class BaseMonitor : IDisposable
    {
        protected FFACE FFACE;
        protected object Lock = new object();
        protected Timer Timer = new Timer();

        protected BaseMonitor(FFACE fface)
            : this()
        {
            FFACE = fface;
        }

        protected BaseMonitor()
        {
            Timer.Elapsed += CheckStatus;
            Timer.AutoReset = true;
            Timer.Interval = 30;
        }

        public bool Enabled
        {
            get { return Timer.Enabled; }
            set { Timer.Enabled = value; }
        }

        public void Dispose()
        {
            Timer.Dispose();
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
            Timer.Start();
        }

        public void Stop()
        {
            Timer.Stop();
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