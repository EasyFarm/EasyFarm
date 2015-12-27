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

using EasyFarm.Classes;
using EasyFarm.Monitors;
using System;
using System.Threading;

namespace EasyFarm.Components
{
    /// <summary>
    /// Controls whether or not the bot should run. Basically anything that can pause or resume the
    /// bot's engine should be here.
    /// </summary>
    public class GameEngine
    {
        /// <summary>
        /// Tells us whether the bot is working or not.
        /// </summary>
        public bool IsWorking;

        /// <summary>
        /// Provides information about game data.
        /// </summary>
        private readonly MemoryWrapper _fface;

        /// <summary>
        /// The engine that controls player actions.
        /// </summary>
        private readonly FiniteStateEngine _stateMachine;


        public GameEngine(MemoryWrapper fface)
        {
            _fface = fface;
            _stateMachine = new FiniteStateEngine(fface);
        }

        /// <summary>
        /// Start the bot up
        /// </summary>
        public void Start()
        {            
            IsWorking = true;
            _stateMachine.Start();
        }

        /// <summary>
        /// Stop the bot from going any further
        /// </summary>
        public void Stop()
        {
            IsWorking = false;
            _stateMachine.Stop();
        }        

        /// <summary>
        /// Pauses the engine when the player zones and resumes after.
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

            EventPublisher.InformUser("Program Paused");

            // Stop the state machine.
            Stop();



            // Start up the state machine again.
            Start();

            EventPublisher.InformUser("Program Resumed");
        }
    }
}