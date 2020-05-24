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
    public class FollowState : BaseState
    {
        public override void Enter(IGameContext context)
        {
            // Stand up from resting. 
            if (context.Player.Status == Status.Healing) Player.Stand(context.API);

            // Disengage an invalid target. 
            if (context.Player.Status == Status.Fighting) Player.Disengage(context.API);
        }

        public override bool Check(IGameContext context)
        {
            // Do not follow during fighting. 
            if (context.IsFighting) return false;

            // Do not follow when resting. 
            if (new RestState().Check(context)) return false;

            // Avoid following empty units. 
            if (string.IsNullOrWhiteSpace(context.Config.FollowedPlayer)) return false;

            // Get the player specified in user settings. 
            var player = context.Memory.UnitService.GetUnitByName(context.Config.FollowedPlayer);

            // If no player is nearby, return. 
            if (player == null) return false;

            // If we're already close, no action needed. 
            return player.Distance > context.Config.FollowDistance;
        }

        public override void Run(IGameContext context)
        {
            // Get the player specified in user settings. 
            var player = context.Memory.UnitService.GetUnitByName(context.Config.FollowedPlayer);

            // Follow the player. 
            var path = context.NavMesh.FindPathBetween(context.API.Player.Position, context.Target.Position);
            if (path.Count > 0 && context.API.Player.Position.Distance(context.Target.Position) > context.Config.FollowDistance)
            {
                context.API.Navigator.DistanceTolerance = 3;

                while (path.Count > 0 && path.Peek().Distance(context.API.Player.Position) <= context.API.Navigator.DistanceTolerance)
                {
                    path.Dequeue();
                }

                context.API.Navigator.GotoNPC(context.Target.Id, path.Peek(), path.Count > 0);
            }
        }
    }
}