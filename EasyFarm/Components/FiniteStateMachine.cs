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

// Author: Myrmidon
// Site: FFEVO.net
// All credit to him!

using System.Linq;
using System.Threading;
using EasyFarm.Classes;
using FFACETools;

namespace EasyFarm.Components
{
    public class FiniteStateEngine : MachineController
    {
        /// <summary>
        ///     A timer for running tasks over long
        ///     periods of time.
        /// </summary>
        private readonly TaskTimer _taskTimer;

        private MachineComponent _lastRan;
        // Constructor.
        public FiniteStateEngine(FFACE fface)
        {
            //Create the states
            AddComponent(new RestComponent(fface) {Priority = 2});
            AddComponent(new AttackContainer(fface) {Priority = 1});
            AddComponent(new TravelComponent(fface) {Priority = 1});
            AddComponent(new HealingComponent(fface) {Priority = 2});
            AddComponent(new EndComponent(fface) {Priority = 3});
            Components.ForEach(x => x.Enabled = true);

            // Threaded timer to run the main loop on. 
            _taskTimer = new TaskTimer();
            _taskTimer.OnElapsed += Run;
            _taskTimer.Interval = 100;
            _taskTimer.AutoReset = true;
        }

        public override bool CheckComponent()
        {
            // Loop through all components and if one reports ready,
            // the attack container may run. 

            var ready = Components.Any(x => x.Enabled && x.CheckComponent());

            return ready;
        }

        private void Run(bool timedOut)
        {
            var acquiredLock = false;

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
                foreach (var mc in Components)
                {
                    // Stop operations on being signaled to stop. 
                    if (!timedOut)
                    {
                        // Make the last state clean up and exit (stops running if travelling)
                        if (_lastRan != null) _lastRan.ExitComponent();
                        return;
                    }

                    if (mc.Enabled == false)
                    {
                        continue;
                    } // Skip disabled States.
                    if (mc.CheckComponent())
                    {
                        // Says it needs to run. Same State as before?
                        if (_lastRan == null)
                        {
                            _lastRan = mc;
                        }
                        if (_lastRan != mc)
                        {
                            // Make the previous State clean up and exit.
                            _lastRan.ExitComponent();
                            _lastRan = mc;
                            mc.EnterComponent();
                        }

                        // Run this State and stop.
                        mc.RunComponent();
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
            _taskTimer.Start();
        }

        public override void Stop()
        {
            _taskTimer.Stop();
        }
    }
}