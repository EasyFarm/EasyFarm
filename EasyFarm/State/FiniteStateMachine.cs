
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013 - 2014>  <Zerolimits>

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
using EasyFarm.State;
using System.Threading.Tasks;
using System.ComponentModel;
using FFACETools;

namespace EasyFarm.State
{
    public class FiniteStateEngine
    {
        private List<BaseState> Brains = new List<BaseState>();
        private BaseState LastRan = null;

        // Timer loop, check the State list.
        private Timer Heartbeat = new Timer();
        private FFACE _fface;

        private BackgroundWorker worker = new BackgroundWorker();

        // Constructor.
        public FiniteStateEngine(FFACE fface)
        {
            Heartbeat.Interval = 100; // Check State list ten times per second.
            Heartbeat.Tick += Heartbeat_Tick;
            worker.DoWork += worker_DoWork;
            worker.WorkerSupportsCancellation = true;

            this._fface = fface;

            //Create the states
            AddState(new RestState(fface));
            AddState(new AttackState(fface));
            AddState(new TravelState(fface));
            AddState(new HealingState(fface));
            AddState(new DeadState(fface));
            AddState(new PostBattle(fface));
            AddState(new TargetInvalid(fface));

            foreach (var b in this.Brains) b.Enabled = true;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (Brains)
            {
                // Sort the List, States may have updated Priorities.
                Brains.Sort();

                // Find a State that says it needs to run.
                foreach (BaseState BS in Brains)
                {
                    // Cancel operations if pause pending.
                    if (worker.CancellationPending) {
                        e.Cancel = true;

                        // Make the last state clean up and exit (stops running if travelling)
                        if (LastRan != null)
                        {
                            LastRan.ExitState();
                        }
                        return; 
                    }

                    if (BS.Enabled == false) { continue; } // Skip disabled States.
                    if (BS.CheckState() == true)
                    {
                        // Says it needs to run. Same State as before?
                        if (LastRan == null) { LastRan = BS; }
                        if (LastRan != BS)
                        {
                            // Make the previous State clean up and exit.
                            LastRan.ExitState();
                            LastRan = BS;
                            BS.EnterState();
                        }

                        // Run this State and stop.
                        BS.RunState();
                    }
                }
            }
        }

        // Handles the updating.
        public void Heartbeat_Tick(object sender, EventArgs e)
        {
            if (!worker.IsBusy) { worker.RunWorkerAsync(); }
        }

        // Start and stop.
        public void Start() 
        { 
            Heartbeat.Start();
        }
        
        public void Stop() 
        { 
            // Stop next round of states. 
            Heartbeat.Stop();
            
            // Prevent next states from executing.
            worker.CancelAsync();
        }

        // Add and remove States.
        public void AddState(BaseState ToAdd) { lock (Brains) { Brains.Add(ToAdd); } }
        public void RemoveState(int index) { lock (Brains) { Brains.RemoveAt(index); } }
    }
}