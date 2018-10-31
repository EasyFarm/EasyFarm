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
using System.Linq;
using EasyFarm.Classes;
using EasyFarm.UserSettings;
using EasyFarm.ViewModels;

namespace EasyFarm.States
{
    public class SetTargetState : AgentState
    {
        private DateTime? lastTargetCheck;

        public SetTargetState(StateMemory memory) : base(memory)
        {
        }

        public override bool Check()
        {
            // Currently fighting, do not change target. 
            if (!UnitFilters.MobFilter(EliteApi, Target, Config))
            {
                // Still not time to update for new target. 
                if (!ShouldCheckTarget()) return false;

                // First get the first mob by distance.
                var mobs = UnitService.MobArray.Where(x => UnitFilters.MobFilter(EliteApi, x, Config)).ToList();
                mobs = TargetPriority.Prioritize(mobs).ToList();

                // Set our new target at the end so that we don't accidentaly cast on a new target.
                Target = mobs.FirstOrDefault();

                // Update last time target was updated. 
                lastTargetCheck = DateTime.Now;

                if (Target != null) LogViewModel.Write("Now targeting " + Target.Name + " : " + Target.Id);
            }

            return false;
        }

        private bool ShouldCheckTarget()
        {
            if (lastTargetCheck == null) return true;
            return DateTime.Now >= lastTargetCheck.Value.AddSeconds(Constants.UnitArrayCheckRate);
        }
    }
}