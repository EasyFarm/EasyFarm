
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
using EasyFarm.Classes;

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
        /// A timer for running tasks over long 
        /// periods of time. 
        /// </summary>
        private TaskTimer TaskTimer = null;

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

            // Threaded timer to run the main loop on. 
            TaskTimer = new TaskTimer();
            TaskTimer.OnElapsed += Run;
            TaskTimer.Interval = 100;
            TaskTimer.AutoReset = true;
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

        private void Run(bool timedOut)
        {
            bool acquiredLock = false;

            try
            {
                // Thread exit if another thread is handling this event. 
                if (Monitor.IsEntered(Components)) return;

                // Acquire access to this resource. 
                Monitor.TryEnter(Components, ref acquiredLock);

                // If we've failed to acquire the lock exit. 
                if (!acquiredLock) return;

                // Sort the List, States may have updated Priorities.
                Components.Sort();

                // Find a State that says it needs to run.
                foreach (MachineComponent MC in Components)
                {
                    // Stop operations on being signaled to stop. 
                    if (!timedOut)
                    {
                        // Make the last state clean up and exit (stops running if travelling)
                        if (LastRan != null) LastRan.ExitComponent();
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
            }
            finally
            {
                if (acquiredLock)
                {
                    // Release our resources. 
                    Monitor.Exit(Components);
                }
            }
        }

        // Start and stop.
        public override void Start()
        {
            TaskTimer.Start();
        }

        public override void Stop()
        {
            TaskTimer.Stop();
        }
    }
}