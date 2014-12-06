
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace EasyFarm.FarmingTool
{
    public class ProcessEventArgs : EventArgs
    {
        public Process Process { get; set; }

        public ProcessEventArgs(Process process)
        {
            this.Process = process;
        }
    }

    public delegate void ProcessEntry(object sender, EventArgs e);

    public delegate void ProcessExit(object sender, EventArgs e);

    public class ProcessWatcher
    {
        /// <summary>
        /// An event that fires when a process has started. 
        /// </summary>
        public event ProcessEntry Entry;

        /// <summary>
        /// An event that fires when a process has exited. 
        /// </summary>
        public event ProcessExit Exit;

        /// <summary>
        /// Internal timer to check process updates. 
        /// </summary>
        protected Timer m_timer = new Timer();

        /// <summary>
        /// List of processes currently monitored. 
        /// </summary>
        public List<Process> Processes { get; set; }

        /// <summary>
        /// Name of the process to monitor. 
        /// </summary>
        public String ProcessName { get; set; }

        /// <summary>
        /// An object for locking.
        /// </summary>
        public Object Mutex { get; set; }

        public ProcessWatcher(String processName)
        {
            Processes = new List<Process>();
            Mutex = new Object();
            this.ProcessName = processName;
            m_timer.Elapsed += CheckProcesses;
            m_timer.AutoReset = true;
            m_timer.Interval = 30;
        }

        protected void CheckProcesses(object sender, ElapsedEventArgs e)
        {
            lock (this.Mutex)
            {
                ///////////////////////////////////////////////////////////////////
                // Check for new processes and fire Entry Events
                ///////////////////////////////////////////////////////////////////
                // Get the list of all running processes. 
                var plist = Process.GetProcessesByName(this.ProcessName).ToList();

                // Get a list of processes that are not contained in "Processes".
                plist = plist.Where(x => !Processes.Any(y => y.Id == x.Id)).ToList();

                // Fire the process entry event for new processes that 
                // have started. 
                foreach (var process in plist)
                {
                    // Add the process
                    this.Processes.Add(process);
                    
                    if (this.Entry != null)
                    {
                        // Fire an entry event. 
                        this.Entry(this, new ProcessEventArgs(process));
                    }
                }

                ///////////////////////////////////////////////////////////////////
                // Check for exiting processes and fire Exit events. 
                ///////////////////////////////////////////////////////////////////

                // Fire the process exit event for old processes that 
                // have exited. 
                foreach (var process in Processes.Where(x => x.HasExited))
                {
                    if (this.Exit != null)
                    {
                        // Fire the process Exit event. 
                        this.Exit(this, new ProcessEventArgs(process));
                    }
                }

                Processes.RemoveAll(x => x.HasExited);
            }
        }

        public void Start() { this.m_timer.Start(); }

        public void Stop() { this.m_timer.Stop(); }
    }
}
