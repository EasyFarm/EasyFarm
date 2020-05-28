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
using Player = EasyFarm.Classes.Player;

namespace EasyFarm.States
{
    public class PullState : BaseState
    {
        /// <summary>
        ///     Allow component to run when moves need to be triggered or
        ///     FightStarted state needs updating.
        /// </summary>
        /// <returns></returns>
        public override bool Check(IGameContext context)
        {
            if (context.IsFighting) return false;
            if (new RestState().Check(context)) return false;
            if (new SummonTrustsState().Check(context)) return false;
            if (!context.Target.IsValid) return false;
            return context.Config.BattleLists["Pull"].Actions.Any(x => x.IsEnabled);
        }

        public override void Enter(IGameContext context)
        {
            context.API.Navigator.Reset();
        }

        /// <summary>
        ///     Use pulling moves if applicable to make the target
        ///     mob aggressive to us.
        /// </summary>
        public override void Run(IGameContext context)
        {
            // Has the user decided we should engage in battle. 
            if (context.Target.Distance <= 25 && context.Config.IsEngageEnabled)
                Player.Engage(context.API);

            var actions = context.Config.BattleLists["Pull"].Actions.ToList();
            var usable = actions.Where(x => ActionFilters.TargetedFilter(context.API, x, context.Target)).ToList();
            context.Memory.Executor.UseTargetedActions(context, usable, context.Target);
        }

        /// <summary>
        ///     Handle all cases of setting fight started to proper values
        ///     so other components can fire.
        /// </summary>
        public override void Exit(IGameContext context)
        {
        }
    }
}