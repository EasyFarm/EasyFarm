
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

namespace EasyFarm.Components
{
    public class FiniteStateEngine : MachineController
    {
        private MachineComponent LastRan = null;

        // Timer loop, check the State list.
        private Timer m_heartBeat = new Timer();

        /// <summary>
        /// FFACE Session for reading memory from the game client. 
        /// </summary>
        private FFACE m_fface;


        private BackgroundWorker m_worker = 
            new BackgroundWorker();


        // Constructor.
        public FiniteStateEngine(FFACE fface)
        {
            m_heartBeat.Interval = 30; // Check State list ten times per second.
            m_heartBeat.Tick += Heartbeat_Tick;
            m_worker.DoWork += Work;
            m_worker.WorkerSupportsCancellation = true;

            this.m_fface = fface;

            //Create the states
            AddComponent(new RestComponent(fface) { Priority = 2 });
            AddComponent(new AttackContainer(fface) { Priority = 1 });
            AddComponent(new TravelComponent(fface) { Priority = 1 });
            AddComponent(new HealingComponent(fface) { Priority = 2 });
            AddComponent(new EndComponent(fface) { Priority = 3 });

            foreach (var b in this.Components)
            {
                b.Enabled = true;
            }
        }

        private void Work(object sender, DoWorkEventArgs e)
        {
            lock (Components)
            {
                // Sort the List, States may have updated Priorities.
                Components.Sort();

                // Find a State that says it needs to run.
                foreach (MachineComponent MC in Components)
                {
                    // Cancel operations if pause pending.
                    if (m_worker.CancellationPending)
                    {
                        e.Cancel = true;

                        // Make the last state clean up and exit (stops running if travelling)
                        if (LastRan != null)
                        {
                            LastRan.ExitComponent();
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
            }
        }

        // Handles the updating.
        public void Heartbeat_Tick(object sender, EventArgs e)
        {
            if (!m_worker.IsBusy) { m_worker.RunWorkerAsync(); }
        }

        // Start and stop.
        public override void Start()
        {
            m_heartBeat.Start();
        }

        public override void Stop()
        {
            // Stop next round of states. 
            m_heartBeat.Stop();

            // Prevent next states from executing.
            m_worker.CancelAsync();
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
    }
}