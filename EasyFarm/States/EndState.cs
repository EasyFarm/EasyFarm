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
using System.Security.Cryptography;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.UserSettings;
using MemoryAPI;
using Player = EasyFarm.Classes.Player;

namespace EasyFarm.States
{
    /// <summary>
    ///     Handles the end of battle situation.
    ///     Fires off the end list, sets FightStart to true so other
    ///     lists can fire and replaces targets that are dead, null,
    ///     empty or invalid.
    /// </summary>
    public class EndState : BaseState
    {
        public override bool Check(IGameContext context)
        {
            // Prevent making the player stand up from resting.
            if (new RestState().Check(context)) return false;

            // Creature is unkillable and does not meets the
            // user's criteria for valid mobs defined in MobFilters.
            return !context.Target.IsValid;
        }

        /// <summary>
        ///     Force player when changing targets.
        /// </summary>
        public override void Enter(IGameContext context)
        {
            context.API.Navigator.Reset();

            while (context.API.Player.Status == Status.Fighting) Player.Disengage(context.API);
        }

        public override void Run(IGameContext context)
        {
            // Execute moves.
            var usable = context.Config.BattleLists["End"].Actions
                .Where(x => ActionFilters.BuffingFilter(context.API, x));

            context.Memory.Executor.UseBuffingActions(usable);

            // Reset all usage data to begin a new battle.
            foreach (var action in context.Config.BattleLists.Actions) action.Usages = 0;

            // dont go to the next state too quickly
            var rando = new Random();
            TimeWaiter.Pause(rando.Next(1800, 9400));
        }
    }
}