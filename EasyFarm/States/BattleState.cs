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
using MemoryAPI;
using System;
using EasyFarm.ViewModels;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using EasyFarm.Context;
using EasyFarm.UserSettings;
using Player = EasyFarm.Classes.Player;

namespace EasyFarm.States
{
    /// <summary>
    ///     A class for defeating monsters.
    /// </summary>
    public class BattleState : BaseState
    {
        public override bool Check(IGameContext context)
        {
            if (new RestState().Check(context)) 
                return false;

            // Make sure we don't need trusts
            if (new SummonTrustsState().Check(context)) 
                return false;

            // Mobs has not been pulled if pulling moves are available. 
            if (!context.IsFighting) 
                return false;

            // target null or dead. 
            if (!context.Target.IsValid) 
                return false;

            // Engage is enabled and we are not engaged. We cannot proceed. 
            if (context.Config.IsEngageEnabled) 
                return context.Player.Status.Equals(Status.Fighting);

            // Engage is not checked, so just proceed to battle. 
            return true;
        }

        public override void Enter(IGameContext context)
        {
            Player.Stand(context.API);
            context.API.Navigator.Reset();
        }

        public override void Run(IGameContext context)
        {
            ShouldRecycleBattleStateCheck(context);
            
            // Cast only one action to prevent blocking curing. 
            var action = context.Config.BattleLists["Battle"].Actions
                .FirstOrDefault(x => ActionFilters.TargetedFilter(context.API, x, context.Target));
            if (action == null) return;
            context.Memory.Executor.UseTargetedActions(context, new[] {action}, context.Target);
        }

        private void ShouldRecycleBattleStateCheck(IGameContext context)
        {
            var chatEntries = context.API.Chat.ChatEntries.ToList();
            var invalidTargetPattern = new Regex("Unable to see");

            List<EliteMMO.API.EliteAPI.ChatEntry> matches = chatEntries
                .Where(x => invalidTargetPattern.IsMatch(x.Text)).ToList();

            foreach (EliteMMO.API.EliteAPI.ChatEntry m in matches.Where(x => x.Timestamp.ToString() == DateTime.Now.ToString()))
            {
                context.API.Windower.SendString(Constants.AttackOff);
                LogViewModel.Write("Recycled battle stance to properly engage the target.");
            }
        }
    }
}