// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013 Mykezero
//  
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
using System;
using System.Linq;
using EasyFarm.Context;

namespace EasyFarm.States
{
    /// <summary>
    ///     Sets up state before other states start firing.
    /// </summary>
    public class StartEngineState : BaseState
    {
        private bool _isAshitaAddonsAndPluginsInitialized = false;
        /// <summary>
        ///     Setup any state before other states start firing.
        /// </summary>
        /// <returns></returns>
        public override bool Check(IGameContext context)
        {
            // Reset all action's last cast times on FSM start. 
            foreach (var action in context.Config.BattleLists.Actions) action.LastCast = DateTime.Now;

            // Only run once at the FSM start. 
            Enabled = false;

            // No need to run body. 
            return !_isAshitaAddonsAndPluginsInitialized;
        }

        public override void Run(IGameContext context)
        {
            //var playerUnit = context.Units.First(x => x.Id == context.Player.Id);
            if (!string.IsNullOrEmpty(context.Player.Name))
            {
                context.API.Windower.SendString("/lw profile " + context.Player.Name.ToLowerInvariant());
            }
            else
            {
                context.API.Windower.SendString("/echo could not find player name ");
            }

            context.API.Navigator.Reset();

            _isAshitaAddonsAndPluginsInitialized = true;
        }
    }
}