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
using System.Linq;
using EasyFarm.Classes;
using EasyFarm.Context;
using MemoryAPI;
using Player = EasyFarm.Classes.Player;

namespace EasyFarm.States
{
    public class HealingState : BaseState
    {
        public override bool Check(IGameContext context)
        {
            if (new RestState().Check(context)) return false;

            return context.Config.BattleLists["Healing"].Actions
                .Any(x => ActionFilters.BuffingFilter(context.API, x));
        }

        public override void Enter(IGameContext context)
        {
            // Stop resting. 
            if (context.Player.Status.Equals(Status.Healing))
                Player.Stand(context.API);

            // Stop moving. 
            context.API.Navigator.Reset();
        }

        public override void Run(IGameContext context)
        {
            // Get the list of healing abilities that can be used.
            var healingMoves = context.Config.BattleLists["Healing"].Actions
                .Where(x => ActionFilters.BuffingFilter(context.API, x))
                .ToList();

            if (healingMoves.Count <= 0) return;
            var healingMove = healingMoves.First();
            context.Memory.Executor.UseBuffingActions(new[] {healingMove});
        }
    }
}