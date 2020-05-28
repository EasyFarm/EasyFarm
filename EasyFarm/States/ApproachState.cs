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
using System.Threading;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.UserSettings;
using MemoryAPI;
using MemoryAPI.Navigation;
using Player = EasyFarm.Classes.Player;

namespace EasyFarm.States
{
    /// <summary>
    ///     Moves to target enemies.
    /// </summary>
    public class ApproachState : BaseState
    {
        public override bool Check(IGameContext context)
        {
            if (new RestState().Check(context)) return false;

            // Make sure we don't need trusts
            if (new SummonTrustsState().Check(context)) return false;

            // Target dead or null.
            if (!context.Target.IsValid) return false;

            // We should approach mobs that have aggroed or have been pulled. 
            if (context.Target.Status.Equals(Status.Fighting)) return true;

            // Get usable abilities. 
            var usable = context.Config.BattleLists["Pull"].Actions
                .Where(x => ActionFilters.BuffingFilter(context.API, x));

            // Approach when there are no pulling moves available. 
            if (!usable.Any()) return true;

            return context.Target.Distance >= Config.Instance.MeleeDistance;
        }

        public override void Enter(IGameContext context)
        {
            Player.Instance.IsStuck = false;
            context.API.Navigator.FaceHeading(context.Target.Position, false);
        }

        public override void Run(IGameContext context)
        {
            // Target mob if not currently targeted. 
            Player.SetTarget(context.API, context.Target);

            // Has the user decided that we should approach targets?
            if (context.Config.IsApproachEnabled)
            {
                // Move to target if out of melee range. 
                var path = context.NavMesh.FindPathBetween(context.API.Player.Position, context.Target.Position);

                // Has the user decided we should engage in battle. 
                if (context.Target.Distance <= 25 && context.Config.IsEngageEnabled)
                    Player.Engage(context.API);

                if (context.Target.Distance <= Config.Instance.MeleeDistance)
                {
                    context.API.Navigator.FaceHeading(context.Target.Position, false);
                    context.API.Navigator.Reset();
                }
                else if (path.Count > 0)
                {
                    context.API.Navigator.DistanceTolerance = 3.0;

                    while (path.Count > 0 && path.Peek().Distance(context.API.Player.Position) <= context.API.Navigator.DistanceTolerance)
                    {
                        path.Dequeue();
                    }
                    
                    if (path.Count > 0)
                    {
                        if (path.Peek().Distance(context.Target.Position) <= Config.Instance.MeleeDistance ||
                            context.API.Player.Position.Distance(context.Target.Position) <= Config.Instance.MeleeDistance) {
                            context.API.Navigator.DistanceTolerance = Config.Instance.MeleeDistance;
                        }
                        context.API.Navigator.GotoNPC(context.Target.Id, path.Peek(), true);
                    }
                    else
                    {
                        context.API.Navigator.GotoNPC(context.Target.Id, context.Target.Position, false);
                    }
                }
            }
        }

        public override void Exit(IGameContext context)
        {
            // Face mob. 
            context.API.Navigator.FaceHeading(context.Target.Position, false);
            context.API.Navigator.Reset();
        }
    }
}