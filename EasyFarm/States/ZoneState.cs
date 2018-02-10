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

using System;
using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.States
{
    public class ZoneState : AgentState
    {
        public Zone Zone;

        public ZoneState(StateMemory stateMemory) : base(stateMemory)
        {
            Zone = EliteApi.Player.Zone;
        }

        public Action ZoningAction { get; set; } = () => TimeWaiter.Pause(500);

        private bool IsZoning => EliteApi.Player.Stats.Str == 0;

        public override bool Check()
        {
            var zone = EliteApi.Player.Zone;
            return ZoneChanged(zone) || IsZoning;
        }

        private bool ZoneChanged(Zone zone)
        {
            return Zone != zone;
        }

        public override void Run()
        {
            // Set new zone.
            Zone = EliteApi.Player.Zone;

            // Stop program from running to next waypoint.
            EliteApi.Navigator.Reset();

            // Wait until we are done zoning.
            while (IsZoning) ZoningAction();
        }
    }
}