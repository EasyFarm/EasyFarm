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
using System.Linq;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.UserSettings;
using EasyFarm.ViewModels;

namespace EasyFarm.States
{
    public class SetTargetState : BaseState
    {
        private DateTime? _lastTargetCheck;

        public override bool Check(IGameContext context)
        {
            // Currently fighting, do not change target. 
            if (!context.Target.IsValid)
            {
                // Still not time to update for new target. 
                if (!ShouldCheckTarget()) return false;

                // First get the first mob by distance.
                var mobs = context.Units.Where(x => x.IsValid).ToList();
                mobs = TargetPriority.Prioritize(mobs).ToList();

                // Set our new target at the end so that we don't accidentally cast on a new target.
                context.Target = mobs.FirstOrDefault() ?? new NullUnit();

                // Update last time target was updated. 
                _lastTargetCheck = DateTime.Now;

                if (context.Target.IsValid)
                {
                    context.Config.Route.ResetCurrentWaypoint();
                    LogViewModel.Write("Now targeting " + context.Target.Name + " : " + context.Target.Id);
                }
            }

            return false;
        }

        private bool ShouldCheckTarget()
        {
            if (_lastTargetCheck == null) return true;
            return DateTime.Now >= _lastTargetCheck.Value.AddSeconds(Constants.UnitArrayCheckRate);
        }
    }
}