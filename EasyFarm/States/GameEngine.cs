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
using MemoryAPI;

namespace EasyFarm.States
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
        private readonly IMemoryAPI _fface;

        /// <summary>
        /// The engine that controls player actions.
        /// </summary>
        private readonly FiniteStateMachine _stateMachine;

        private readonly PlayerMonitor _playerMonitor;

        public GameEngine(IMemoryAPI fface)
        {
            _fface = fface;
            _stateMachine = new FiniteStateMachine(fface);
            _playerMonitor = new PlayerMonitor(fface);
        }

        /// <summary>
        /// Start the bot up
        /// </summary>
        public bool Start()
        {
            var route = Config.Instance.Route;
            var isPathReachable = !route.IsPathSet || route.IsPathUnreachable(_fface);

            if (isPathReachable)
            {
                IsWorking = true;
                _stateMachine.Start();
                _playerMonitor.Start();
                return true;
            }

            AppServices.InformUser("The route is not reachable");
            return false;
        }

        /// <summary>
        /// Stop the bot from going any further
        /// </summary>
        public void Stop()
        {
            IsWorking = false;
            _stateMachine.Stop();
            _playerMonitor.Stop();
        }                
    }
}