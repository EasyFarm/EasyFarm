
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

﻿// Author: Myrmidon
// Site: FFEVO.net
// All credit to him!

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EasyFarm.Components;
using System.Threading.Tasks;
using System.ComponentModel;
using FFACETools;
using System.Threading;

namespace EasyFarm.Components
{
    public class FiniteStateEngine : MachineController
    {
        private MachineComponent LastRan = null;

        /// <summary>
        /// FFACE Session for reading memory from the game client. 
        /// </summary>
        private FFACE m_fface;

        /// <summary>
        /// Our operational state object. 
        /// </summary>
        private TaskInfo taskInfo = null;

        // Constructor.
        public FiniteStateEngine(FFACE fface)
        {
            this.m_fface = fface;

            //Create the states
            AddComponent(new RestComponent(fface) { Priority = 2 });
            AddComponent(new AttackContainer(fface) { Priority = 1 });
            AddComponent(new TravelComponent(fface) { Priority = 1 });
            AddComponent(new HealingComponent(fface) { Priority = 2 });
            AddComponent(new EndComponent(fface) { Priority = 3 });
            this.Components.ForEach(x => x.Enabled = true);
        }

        private void Work(object state, bool timedOut)
        {
            // Keep looping until user requests cancel. 
            while (!((TaskInfo)state).IsCanceled)
            {
                // Sort the List, States may have updated Priorities.
                Components.Sort();

                // Find a State that says it needs to run.
                foreach (MachineComponent MC in Components)
                {
                    // Stop operations on being signaled to stop. 
                    if (!timedOut)
                    {
                        // Make the last state clean up and exit (stops running if travelling)
                        if (LastRan != null)
                        {
                            LastRan.ExitComponent();
                        }

                        if (taskInfo.Handle != null)
                        {
                            taskInfo.Handle.Unregister(null);
                        }

                        return;
                    }

                    if (MC.Enabled == false) { continue; } // Skip disabled States.
                    if (MC.CheckComponent() == true)
                    {
                        // Says it needs to run. Same State as before?
                        if (LastRan == null) { LastRan = MC; }
                        if (LastRan != MC)
                        {
                            // Make the previous State clean up and exit.
                            LastRan.ExitComponent();
                            LastRan = MC;
                            MC.EnterComponent();
                        }

                        // Run this State and stop.
                        MC.RunComponent();
                    }
                }

                // Acts like timer's elapsed wait duration. 
                Thread.Sleep(100);
            }
        }

        // Start and stop.
        public override void Start()
        {
            taskInfo = new TaskInfo(Work);
        }

        public override void Stop()
        {
            taskInfo.Dispose();
        }

        public override bool CheckComponent()
        {
            var ready = false;

            // Loop through all components and if one reports ready,
            // the attack container may run. 
            foreach (var Component in this.Components)
            {
                if (Component.Enabled)
                {
                    ready |= Component.CheckComponent();
                }
            }

            return ready;
        }

        public class TaskInfo : IDisposable
        {
            public RegisteredWaitHandle Handle = null;

            /// <summary>
            /// Cancels running new states. 
            /// </summary>
            public bool IsCanceled = false;

            /// <summary>
            /// Signals this task to stop. 
            /// </summary>
            public AutoResetEvent AutoReset = new AutoResetEvent(false);

            public TaskInfo(WaitOrTimerCallback callback)
            {
                Handle = ThreadPool.RegisterWaitForSingleObject(
                    AutoReset,
                    new WaitOrTimerCallback(callback),
                    this,
                    500,
                    true
                );
            }

            public void Dispose()
            {
                IsCanceled = true;
                if (Handle != null) Handle.Unregister(null);
                AutoReset.Dispose();
            }
        }
    }
}