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
using EasyFarm.UserSettings;
using MemoryAPI;

namespace EasyFarm.States
{
    /// <summary>
    ///     Buffs the player.
    /// </summary>
    public class StartState : BaseState
    {
        public override bool Check(IGameContext context)
        {
            if (new RestState().Check(context)) return false;

            // target dead or null. 
            if (!context.Target.IsValid) return false;

            // Return true if fight has not started. 
            return !context.Target.Status.Equals(Status.Fighting);
        }

        public override void Enter(IGameContext context)
        {
            context.API.Navigator.Reset();
        }

        public override void Run(IGameContext context)
        {
            var usable = context.Config.BattleLists["Start"]
                .Actions.Where(x => ActionFilters.BuffingFilter(context.API, x));

            // Execute moves at target. 
            context.Memory.Executor.UseBuffingActions(usable);
        }
    }
}