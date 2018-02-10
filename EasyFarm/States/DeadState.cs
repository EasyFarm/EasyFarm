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
using EliteMMO.API;
using MemoryAPI;

namespace EasyFarm.States
{
    public class DeadState : AgentState
    {
        public DeadState(StateMemory stateMemory) : base(stateMemory)
        {
        }

        public override bool Check()
        {
            var status = EliteApi.Player.Status;
            return status == Status.Dead1 || status == Status.Dead2;
        }

        public override void Run()
        {
            // Stop program from running to next waypoint.
            EliteApi.Navigator.Reset();

            if (Config.Instance.HomePointOnDeath) HomePointOnDeath();

            // Stop the engine from running.
            AppServices.SendPauseEvent();
        }

        private void HomePointOnDeath()
        {
            TimeWaiter.Pause(2000);
            EliteApi.Windower.SendKeyPress(Keys.NUMPADENTER);
            TimeWaiter.Pause(1000);
            EliteApi.Windower.SendKeyPress(Keys.LEFT);
            TimeWaiter.Pause(1000);
            EliteApi.Windower.SendKeyPress(Keys.NUMPADENTER);
        }
    }
}