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
using System;
using EasyFarm.Classes;
using EasyFarm.Context;
using MemoryAPI;

namespace EasyFarm.States
{
    public class ZoneState : BaseState
    {
        public Action ZoningAction { get; set; } = () => TimeWaiter.Pause(500);

        private bool IsZoning(IGameContext context) => context.Player.Str == 0;

        public override void Enter(IGameContext context)
        {
            if (context.Zone == Zone.Unknown)
            {
                context.Zone = context.Player.Zone;
            }
        }

        public override bool Check(IGameContext context)
        {
            var zone = context.Player.Zone;
            return ZoneChanged(zone, context.Zone) || IsZoning(context);
        }

        private bool ZoneChanged(Zone currentZone, Zone lastZone)
        {
            return lastZone != currentZone;
        }

        public override void Run(IGameContext context)
        {
            // Set new currentZone.
            context.Zone = context.Player.Zone;

            // Stop program from running to next waypoint.
            context.API.Navigator.Reset();

            // Wait until we are done zoning.
            while (IsZoning(context)) ZoningAction();
        }
    }
}