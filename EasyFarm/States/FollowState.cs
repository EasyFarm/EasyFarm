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

using EasyFarm.Classes;
using EasyFarm.UserSettings;
using MemoryAPI;

namespace EasyFarm.States
{
    /// <summary>
    ///     Moves to target enemies.
    /// </summary>
    public class FollowState : AgentState
    {
        public FollowState(StateMemory memory) : base(memory)
        {
        }

        public override void Enter()
        {
            // Stand up from resting. 
            if (EliteApi.Player.Status == Status.Healing) Player.Stand(EliteApi);

            // Disengage an invalid target. 
            if (EliteApi.Player.Status == Status.Fighting) Player.Disengage(EliteApi);
        }

        public override bool Check()
        {
            // Do not follow during fighting. 
            if (IsFighting) return false;

            // Do not follow when resting. 
            if (new RestState(Memory).Check()) return false;

            // Avoid following empty units. 
            if (string.IsNullOrWhiteSpace(Config.Instance.FollowedPlayer)) return false;

            // Get the player specified in user settings. 
            var player = UnitService.GetUnitByName(Config.Instance.FollowedPlayer);

            // If no player is nearby, return. 
            if (player == null) return false;

            // If we're already close, no action needed. 
            return player.Distance > Config.Instance.FollowDistance;
        }

        public override void Run()
        {
            // Get the player specified in user settings. 
            var player = UnitService.GetUnitByName(Config.Instance.FollowedPlayer);

            // Follow the player. 
            EliteApi.Navigator.DistanceTolerance = Config.Instance.FollowDistance;
            EliteApi.Navigator.GotoNPC(player.Id, Config.Instance.IsObjectAvoidanceEnabled);
        }
    }
}