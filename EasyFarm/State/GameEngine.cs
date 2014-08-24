
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
        /// Stops the bot on zone and re-enables it on re-zone. 
        /// </summary>
        private Timer ZoneTimer = new Timer();

        /// <summary>
        /// Keeps the gui thread from freezing. 
        /// </summary>
        private BackgroundWorker Worker = new BackgroundWorker();

        /// <summary>
        /// Holds the previous zone. Used to stop the bot on zone. 
        /// </summary>
        private Zone _zone = new Zone();

        private FFACE _fface;

        public GameEngine(FFACE fface)
        {
            this._fface = fface;
            this._zone = fface.Player.Zone;

            StateMachine = new FiniteStateEngine(fface);
            ZoneTimer.Tick += ZoneTimer_Tick;
            ZoneTimer.Enabled = true;
            ZoneTimer.Interval = 100;
            Worker.DoWork += Worker_DoWork;
        }

        void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // If the program is not running or the zone is the same
            // bail out. 
            if (!IsWorking || _zone == _fface.Player.Zone)
            {
                _zone = _fface.Player.Zone;
                return;
            }

            App.InformUser("Program Paused");

            // Stop the state machine.
            StateMachine.Stop();

            // Set up waiting of 5 seconds to be our current time + 10 seconds. 
            var waitDelay = DateTime.Now.Add(TimeSpan.FromSeconds(10));

            // Wait for five seconds after zoning. 
            while (DateTime.Now < waitDelay) { }

            // Start up the state machine again.
            StateMachine.Start();

            // Set our zone to our new zone. 
            _zone = _fface.Player.Zone;

            App.InformUser("Program Resumed");
        }

        void ZoneTimer_Tick(object sender, EventArgs e)
        {
            if (!Worker.IsBusy) Worker.RunWorkerAsync();
        }

        public FiniteStateEngine StateMachine { get; set; }

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
    }
}
