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
using EasyFarm.Classes;
using EasyFarm.Context;
using EliteMMO.API;
using MemoryAPI;

namespace EasyFarm.States
{
    public class DeadState : BaseState
    {
        public override bool Check(IGameContext context)
        {
            var status = context.Player.Status;
            return status == Status.Dead1 || status == Status.Dead2;
        }

        public override void Run(IGameContext context)
        {
            // Stop program from running to next waypoint.
            context.API.Navigator.Reset();

            if (context.Config.HomePointOnDeath) HomePointOnDeath(context);

            // Stop the engine from running.
            AppServices.SendPauseEvent();
        }

        private void HomePointOnDeath(IGameContext context)
        {
            TimeWaiter.Pause(2000);
            context.API.Windower.SendKeyPress(Keys.NUMPADENTER);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(Keys.LEFT);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(Keys.NUMPADENTER);
        }
    }
}