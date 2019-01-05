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
using System.Collections.Generic;
using System.Linq;
using EasyFarm.Classes;
using EasyFarm.Context;
using MemoryAPI;
using Player = EasyFarm.Classes.Player;

namespace EasyFarm.States
{
    /// <summary>
    ///     Behavior for resting our character.
    /// </summary>
    public class RestState : BaseState
    {
        /// <summary>
        ///     Determines if we should rest up our health or magic.
        /// </summary>
        /// <returns></returns>
        public override bool Check(IGameContext context)
        {
            // Check for effects taht stop resting. 
            if (ProhibitEffects.ProhibitEffectsDots
                .Intersect(context.API.Player.StatusEffects).Any()) 
                return false;

            // Do not rest if we are being attacked. 
            if (context.Player.HasAggro) 
                return false;

            // Check if we are fighting. 
            if (context.Player.Status == Status.Fighting) 
                return false;

            // Check if we should rest for health.
            if (ShouldRestForHealth(
                context.Player.HppCurrent,
                context.Player.Status,
                context.Config.IsHealthEnabled,
                context.Config.LowHealth,
                context.Config.HighHealth)) 
                return true;

            // Check if we should rest for magic. 
            if (ShouldRestForMagic(
                context.Player.MppCurrent,
                context.Player.Status,
                context.Config.IsMagicEnabled,
                context.Config.LowMagic,
                context.Config.HighHealth)) 
                return true;

            // We do not meet the conditions for resting. 
            return false;
        }

        /// <summary>
        ///     Begin resting our health and magic.
        /// </summary>
        public override void Run(IGameContext context)
        {
            Player.Rest(context.API);
        }

        /// <summary>
        ///     Force the player to stand up before attempting anything else.
        /// </summary>
        public override void Exit(IGameContext context)
        {
            while (context.API.Player.Status == Status.Healing) Player.Stand(context.API);
        }

        /// <summary>
        ///     Tells us when we should rest mp.
        /// </summary>
        /// <param name="magic"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool ShouldRestForMagic(int magic, Status status, bool enabled, int lowMagic, int highMagic)
        {
            return enabled &&
                   (IsMagicLow(magic, lowMagic) || !IsMagicHigh(magic, highMagic) && status == Status.Healing);
        }

        /// <summary>
        ///     Given a value, is our mp low?
        /// </summary>
        /// <param name="magic"></param>
        /// <returns></returns>
        public bool IsMagicLow(int magic, int lowMagic)
        {
            return magic <= lowMagic;
        }

        /// <summary>
        ///     Given a value, is our mp high?
        /// </summary>
        /// <param name="magic"></param>
        /// <returns></returns>
        public bool IsMagicHigh(int magic, int highMagic)
        {
            return magic >= highMagic;
        }

        /// <summary>
        ///     Should we rest up our magic?
        /// </summary>
        /// <param name="health"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool ShouldRestForHealth(int health, Status status, bool enabled, int lowHealth, int highHealth)
        {
            // Rest while low and while not high
            return enabled && (IsHealthLow(health, lowHealth) || !IsHealthHigh(health, highHealth) && status == Status.Healing);
        }

        /// <summary>
        ///     Given a value, is our health low?
        /// </summary>
        /// <param name="health"></param>
        /// <returns></returns>
        public bool IsHealthLow(int health, int lowHealth)
        {
            return health <= lowHealth;
        }

        /// <summary>
        ///     Given a value, is our health high?
        /// </summary>
        /// <param name="health"></param>
        /// <returns></returns>
        public bool IsHealthHigh(int health, int highHealth)
        {
            return health >= highHealth;
        }
    }
}