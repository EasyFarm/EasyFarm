
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

using EasyFarm.FarmingTool;
using FFACETools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace EasyFarm.State
{
    /// <summary>
    /// An object that controls the bot and contains all the data. 
    /// The bot can access everything it needs from this object.
    /// </summary>
    public class GameEngine
    {
        /// <summary>
        /// Tells us whether the bot is working or not.
        /// </summary>
        public bool IsWorking = false;

        /// <summary>
        /// Monitors for zone changes and allows for pausing / resuming
        /// the program after zoning. 
        /// </summary>
        private ZoneMonitor _zoneMonitor;

        /// <summary>
        /// Monitors for players nearby and allows for pausing / resuming
        /// the program on detection. 
        /// </summary>
        private PlayerMonitor _playerMonitor;

        private FFACE _fface;

        public GameEngine(FFACE fface)
        {
            this._fface = fface;
            this._zoneMonitor = new ZoneMonitor(fface);
            this._playerMonitor = new PlayerMonitor(fface);
            this.StateMachine = new FiniteStateEngine(fface);

            _zoneMonitor.Changed += ZoneMonitor_ZoneChanged;
            _zoneMonitor.Start();

            _playerMonitor.Changed += PlayerMonitor_DetectedChanged;
            _playerMonitor.Start();
        }

        private void PlayerMonitor_DetectedChanged(object sender, EventArgs e)
        {
            // If the program is not running then bail out. 
            if (!IsWorking) { return; }

            var args = (e as MonitorArgs<bool>);
            if (args.Status)
            {
                App.InformUser("Program Paused");
                Stop();
            }
            else 
            {
                App.InformUser("Program Resumed");
                Start();
            }
        }

        private void ZoneMonitor_ZoneChanged(object sender, EventArgs e)
        {
            var args = (e as MonitorArgs<Zone>);

            // If the program is not running then bail out. 
            if (!IsWorking) { return; }

            App.InformUser("Program Paused");

            // Stop the state machine.
            Stop();

            // Set up waiting of 5 seconds to be our current time + 10 seconds. 
            var waitDelay = DateTime.Now.Add(TimeSpan.FromSeconds(10));

            // Wait for five seconds after zoning. 
            while (DateTime.Now < waitDelay)  { }

            // Start up the state machine again.
            Start();

            App.InformUser("Program Resumed");
        }

        /// <summary>
        /// Start the bot up
        /// </summary>
        public void Start()
        {
            StateMachine.Start();
            IsWorking = true;
        }

        /// <summary>
        /// Stop the bot from going any further
        /// </summary>
        public void Stop()
        {
            StateMachine.Stop();
            IsWorking = false;
        }

        public FiniteStateEngine StateMachine { get; set; }
    }
}
