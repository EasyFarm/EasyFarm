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
    ///     Handles the end of battle situation.
    ///     Fires off the end list, sets FightStart to true so other
    ///     lists can fire and replaces targets that are dead, null,
    ///     empty or invalid.
    /// </summary>
    public class EndState : AgentState
    {
        public EndState(StateMemory memory) : base(memory)
        {
        }

        public override bool Check()
        {
            // Prevent making the player stand up from resting.
            if (new RestState(Memory).Check()) return false;

            // Creature is unkillable and does not meets the
            // user's criteria for valid mobs defined in MobFilters.
            return !UnitFilters.MobFilter(EliteApi, Target);
        }

        /// <summary>
        ///     Force player when changing targets.
        /// </summary>
        public override void Enter()
        {
            EliteApi.Navigator.Reset();

            while (EliteApi.Player.Status == Status.Fighting) Player.Disengage(EliteApi);
        }

        public override void Run()
        {
            // Execute moves.
            var usable = Config.Instance.BattleLists["End"].Actions
                .Where(x => ActionFilters.BuffingFilter(EliteApi, x));

            Executor.UseBuffingActions(usable);

            // Reset all usage data to begin a new battle.
            foreach (var action in Config.Instance.BattleLists.Actions) action.Usages = 0;
        }
    }
}