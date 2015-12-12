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

namespace EasyFarm.Monitors
{
    /// <summary>
    /// Fired when processes enter the system.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ProcessEntry(object sender, EventArgs e);

    /// <summary>
    /// Fires when processes exit the system.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ProcessExit(object sender, EventArgs e);

    /// <summary>
    /// Encapsulates the process that has entered / exited the system.
    /// </summary>
    public class ProcessEventArgs : EventArgs
    {
        public ProcessEventArgs(Process process)
        {
            Process = process;
        }

        public Process Process { get; set; }
    }
    /// <summary>
    /// Keeps track of processes with a given name.
    /// </summary>
    public class ProcessWatcher
    {
        /// <summary>
        /// Internal timer to check process updates.
        /// </summary>
        protected BackgroundWorker backgroundWorker = new BackgroundWorker();

        public ProcessWatcher(string processName)
        {
            backgroundWorker.DoWork += Bgw_DoWork;
            backgroundWorker.WorkerSupportsCancellation = true;
            Processes = new List<Process>();
            ProcessName = processName;
        }

        /// <summary>
        /// An event that fires when a process has started.
        /// </summary>
        public event ProcessEntry Entry;

        /// <summary>
        /// An event that fires when a process has exited.
        /// </summary>
        public event ProcessExit Exit;

        /// <summary>
        /// List of processes currently monitored.
        /// </summary>
        public List<Process> Processes { get; set; }

        /// <summary>
        /// Name of the process to monitor.
        /// </summary>
        public string ProcessName { get; set; }
        /// <summary>
        /// Start monitoring processes.
        /// </summary>
        public void Start()
        {
            backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Stop monitoring processes.
        /// </summary>
        public void Stop()
        {
            backgroundWorker.CancelAsync();
        }

        /// <summary>
        /// Fire process enter events for new processes. 
        /// </summary>
        /// <param name="processes"></param>
        protected void DoProcessEnter(List<Process> processes)
        {
            // Fire the process entry event for new processes that
            // have started.
            foreach (var process in processes)
            {
                // Add the process
                Processes.Add(process);

                if (Entry != null)
                {
                    // Fire an entry event.
                    Entry(this, new ProcessEventArgs(process));
                }
            }
        }

        /// <summary>
        /// Run processing on all existing and new processes. 
        /// </summary>
        protected void DoProcesses()
        {
            var processes = GetNewProcesses();
            DoProcessEnter(processes);
            DoProcessExit();
        }

        /// <summary>
        /// Fire all process exit events and remove the process from our list. 
        /// </summary>
        protected void DoProcessExit()
        {
            try
            {
                foreach (var process in Processes)
                {
                    if (process.HasExited)
                    {
                        if (Exit != null)
                        {
                            // Fire the process Exit event.
                            Exit(this, new ProcessEventArgs(process));
                        }

                        Processes.Remove(process);
                    }
                }
            }
            catch (Exception)
            {
                // Non-Critical Error Trying to retrieve a process; most likely
                // a system process we do not have access to.
            }
        }

        /// <summary>
        /// Get's the list of all new processes. 
        /// </summary>
        /// <returns></returns>
        protected List<Process> GetNewProcesses()
        {
            var processes = (string.IsNullOrEmpty(ProcessName) ? Process.GetProcesses() : Process.GetProcessesByName(ProcessName)).ToList();

            processes = processes.Where(x => Processes.All(y => y.Id != x.Id))
                .Where(x => !string.IsNullOrWhiteSpace(x.MainWindowTitle))
                .ToList();

            return processes;
        }

        private void Bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker.CancellationPending)
            {
                DoProcesses();
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}