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
    ///     Performs weaponskills on targets.
    /// </summary>
    public class WeaponskillState : AgentState
    {
        public WeaponskillState(StateMemory memory) : base(memory)
        {
        }

        public override bool Check()
        {
            if (new RestState(Memory).Check()) return false;

            if (!UnitFilters.MobFilter(EliteApi, Target)) return false;

            // Use skill if we are engaged. 
            return EliteApi.Player.Status.Equals(Status.Fighting);
        }

        public override void Run()
        {
            // Check engaged
            // FIXED: no longer return on not engage but don't execute 
            // these moves instead. Fixes the bot not attacking things 
            // from move than 30 yalms problem. 
            if (!EliteApi.Player.Status.Equals(Status.Fighting)) return;
            var weaponskill = Config.Instance.BattleLists["Weaponskill"].Actions
                .FirstOrDefault(x => ActionFilters.TargetedFilter(EliteApi, x, Target));
            if (weaponskill == null) return;
            Executor.UseTargetedActions(new[] {weaponskill}, Target);
        }
    }
}