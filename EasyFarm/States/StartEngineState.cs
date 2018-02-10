// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
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
using EasyFarm.UserSettings;

namespace EasyFarm.States
{
    /// <summary>
    ///     Sets up state before other states start firing.
    /// </summary>
    public class StartEngineState : AgentState
    {
        public StartEngineState(StateMemory memory) : base(memory)
        {
        }

        /// <summary>
        ///     Setup any state before other states start firing.
        /// </summary>
        /// <returns></returns>
        public override bool Check()
        {
            // Reset all action's last cast times on FSM start. 
            foreach (var action in Config.Instance.BattleLists.Actions) action.LastCast = DateTime.Now;

            // Only run once at the FSM start. 
            Enabled = false;

            // No need to run body. 
            return false;
        }
    }
}