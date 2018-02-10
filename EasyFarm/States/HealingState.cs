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
    public class HealingState : AgentState
    {
        public HealingState(StateMemory fface) : base(fface)
        {
        }

        public override bool Check()
        {
            if (new RestState(Memory).Check()) return false;

            return Config.Instance.BattleLists["Healing"].Actions
                .Any(x => ActionFilters.BuffingFilter(EliteApi, x));
        }

        public override void Enter()
        {
            // Stop resting. 
            if (EliteApi.Player.Status.Equals(Status.Healing))
                Player.Stand(EliteApi);

            // Stop moving. 
            EliteApi.Navigator.Reset();
        }

        public override void Run()
        {
            // Get the list of healing abilities that can be used.
            var healingMoves = Config.Instance.BattleLists["Healing"].Actions
                .Where(x => ActionFilters.BuffingFilter(EliteApi, x))
                .ToList();

            if (healingMoves.Count <= 0) return;
            var healingMove = healingMoves.First();
            Executor.UseBuffingActions(new[] {healingMove});
        }
    }
}