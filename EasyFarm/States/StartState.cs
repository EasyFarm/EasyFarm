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

using System.Linq;
using EasyFarm.Classes;
using EasyFarm.UserSettings;
using MemoryAPI;

namespace EasyFarm.States
{
    /// <summary>
    ///     Buffs the player.
    /// </summary>
    public class StartState : AgentState
    {
        public StartState(StateMemory memory) : base(memory)
        {
        }

        public override bool Check()
        {
            if (new RestState(Memory).Check()) return false;

            // target dead or null. 
            if (!UnitFilters.MobFilter(EliteApi, Target)) return false;

            // Return true if fight has not started. 
            return !Target.Status.Equals(Status.Fighting);
        }

        public override void Enter()
        {
            EliteApi.Navigator.Reset();
        }

        public override void Run()
        {
            var usable = Config.Instance.BattleLists["Start"]
                .Actions.Where(x => ActionFilters.BuffingFilter(EliteApi, x));

            // Execute moves at target. 
            Executor.UseBuffingActions(usable);
        }
    }
}