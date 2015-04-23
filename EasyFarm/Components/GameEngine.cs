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
using System.Threading;
using EasyFarm.Classes;
using EasyFarm.Monitors;
using FFACETools;

namespace EasyFarm.Components
{
    /// <summary>
    ///     Controls whether or not the bot should run. Basically anything
    ///     that can pause or resume the bot's engine should be here.
    /// </summary>
    public class GameEngine
    {
        /// <summary>
        ///     Monitors the player status to see if their dead. If so, the monitor
        ///     will stop the program from running.
        /// </summary>
        private readonly DeadMonitor _deadMonitor;

        /// <summary>
        ///     Provides information about game data.
        /// </summary>
        private readonly FFACE _fface;

        /// <summary>
        ///     The engine that controls player actions.
        /// </summary>
        private readonly FiniteStateEngine _stateMachine;

        /// <summary>
        ///     Monitors for zone changes and allows for pausing / resuming
        ///     the program after zoning.
        /// </summary>
        private readonly ZoneMonitor _zoneMonitor;

        /// <summary>
        ///     Tells us whether the bot is working or not.
        /// </summary>
        public bool IsWorking;

        public GameEngine(FFACE fface)
        {
            _fface = fface;
            _zoneMonitor = new ZoneMonitor(fface);
            _deadMonitor = new DeadMonitor(fface);
            _stateMachine = new FiniteStateEngine(fface);

            _zoneMonitor.Changed += ZoneMonitorZoneChanged;
            _zoneMonitor.Start();

            _deadMonitor.Changed += DeadMonitorStatusChanged;
            _deadMonitor.Start();
        }

        /// <summary>
        ///     Monitors engine status for player being stuck and
        ///     shuts it down when detected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void StuckMonitor_StuckChanged(object sender, EventArgs e)
        {
            // Do nothing if the engine is not running already. 
            if (!IsWorking)
            {
                return;
            }

            // Stop program from running to next waypoint. 
            _fface.Navigator.Reset();

            // Tell the use we paused the program. 
            AppInformer.InformUser("Program Paused");

            // Stop the engine from running. 
            Stop();
        }

        /// <summary>
        ///     Monitors engine stats for player dead status
        ///     and then shuts down the engine.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeadMonitorStatusChanged(object sender, EventArgs e)
        {
            // Do nothing if the engine is not running already. 
            if (!IsWorking)
            {
                return;
            }

            // Stop program from running to next waypoint. 
            _fface.Navigator.Reset();

            // Tell the use we paused the program. 
            AppInformer.InformUser("Program Paused");

            // Stop the engine from running. 
            Stop();
        }

        /// <summary>
        ///     Stops the engine when another player is detected and
        ///     resumes afterwards.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PlayerMonitor_DetectedChanged(object sender, EventArgs e)
        {
            // If the program is not running then bail out. 
            if (!IsWorking)
            {
                return;
            }

            var args = (e as MonitorArgs<bool>);
            if (args != null && args.Status)
            {
                AppInformer.InformUser("Program Paused");
                Stop();
            }
            else
            {
                AppInformer.InformUser("Program Resumed");
                Start();
            }
        }

        /// <summary>
        ///     Pauses the engine when the player zones and
        ///     resumes after.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoneMonitorZoneChanged(object sender, EventArgs e)
        {
            // If the program is not running then bail out. 
            if (!IsWorking)
            {
                return;
            }

            AppInformer.InformUser("Program Paused");

            // Stop the state machine.
            Stop();

            // Wait until our player has zoned; 
            while (_fface.Player.Stats.Str == 0)
            {
                Thread.Sleep(500);
            }

            // Start up the state machine again.
            Start();

            AppInformer.InformUser("Program Resumed");
        }

        /// <summary>
        ///     Start the bot up
        /// </summary>
        public void Start()
        {
            _stateMachine.Start();
            IsWorking = true;
        }

        /// <summary>
        ///     Stop the bot from going any further
        /// </summary>
        public void Stop()
        {
            _stateMachine.Stop();
            IsWorking = false;
        }
    }
}