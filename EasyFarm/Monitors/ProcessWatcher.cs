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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace EasyFarm.Classes
{
    public class ProcessEventArgs : EventArgs
    {
        public ProcessEventArgs(Process process)
        {
            Process = process;
        }

        public Process Process { get; set; }
    }

    public delegate void ProcessEntry(object sender, EventArgs e);

    public delegate void ProcessExit(object sender, EventArgs e);

    public class ProcessWatcher : IDisposable
    {
        /// <summary>
        ///     Internal timer to check process updates.
        /// </summary>
        protected Timer m_timer = new Timer();

        public ProcessWatcher(string processName)
        {
            Processes = new List<Process>();
            Mutex = new object();
            ProcessName = processName;
            m_timer.Elapsed += CheckProcesses;
            m_timer.AutoReset = true;
            m_timer.Interval = 30;
        }

        /// <summary>
        ///     List of processes currently monitored.
        /// </summary>
        public List<Process> Processes { get; set; }

        /// <summary>
        ///     Name of the process to monitor.
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        ///     An object for locking.
        /// </summary>
        public object Mutex { get; set; }

        public void Dispose()
        {
            m_timer.Dispose();
        }

        /// <summary>
        ///     An event that fires when a process has started.
        /// </summary>
        public event ProcessEntry Entry;

        /// <summary>
        ///     An event that fires when a process has exited.
        /// </summary>
        public event ProcessExit Exit;

        protected void CheckProcesses(object sender, ElapsedEventArgs e)
        {
            lock (Mutex)
            {
                ///////////////////////////////////////////////////////////////////
                // Check for new processes and fire Entry Events
                ///////////////////////////////////////////////////////////////////
                // Get the list of all running processes. 

                var plist = (string.IsNullOrWhiteSpace(ProcessName)
                    ? Process.GetProcesses()
                    : Process.GetProcessesByName(ProcessName)).ToList();

                // Get a list of processes that are not contained in "Processes".
                plist = plist.Where(x => !Processes.Any(y => y.Id == x.Id)).ToList();

                // Fire the process entry event for new processes that 
                // have started. 
                foreach (var process in plist)
                {
                    // Add the process
                    Processes.Add(process);

                    if (Entry != null)
                    {
                        // Fire an entry event. 
                        Entry(this, new ProcessEventArgs(process));
                    }
                }

                ///////////////////////////////////////////////////////////////////
                // Check for exiting processes and fire Exit events. 
                ///////////////////////////////////////////////////////////////////

                // Fire the process exit event for old processes that 
                // have exited. 
                try
                {
                    foreach (var process in Processes.Where(x => x.HasExited))
                    {
                        if (Exit != null)
                        {
                            // Fire the process Exit event. 
                            Exit(this, new ProcessEventArgs(process));
                        }
                    }

                    Processes.RemoveAll(x => x.HasExited);
                }
                catch (Win32Exception)
                {
                    // Non-Critical Error Trying to retrieve a process; most likely
                    // a system process we do not have access to. 
                }
            }
        }

        public void Start()
        {
            m_timer.Start();
        }

        public void Stop()
        {
            m_timer.Stop();
        }
    }
}