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
using MemoryAPI;
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
            context.API.Navigator.DistanceTolerance = context.Config.FollowDistance;
            context.API.Navigator.GotoNPC(player.Id, context.Config.IsObjectAvoidanceEnabled);
        }
    }
}