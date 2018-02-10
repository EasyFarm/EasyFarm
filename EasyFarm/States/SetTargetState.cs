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
        private readonly UnitService _units;
        private DateTime _lastTargetCheck = DateTime.Now;

        public SetTargetState(StateMemory memory) : base(memory)
        {
            _units = new UnitService(EliteApi);
        }

        public override bool Check()
        {
            // Currently fighting, do not change target. 
            if (!UnitFilters.MobFilter(EliteApi, Target))
            {
                // Still not time to update for new target. 
                if (DateTime.Now < _lastTargetCheck.AddSeconds(Constants.UnitArrayCheckRate)) return false;

                // First get the first mob by distance.
                var mobs = _units.MobArray.Where(x => UnitFilters.MobFilter(EliteApi, x))
                    .OrderByDescending(x => x.PartyClaim)
                    .ThenByDescending(x => x.HasAggroed)
                    .ThenBy(x => x.Distance)
                    .ToList();

                // Set our new target at the end so that we don't accidentaly cast on a new target.
                Target = mobs.FirstOrDefault();

                // Update last time target was updated. 
                _lastTargetCheck = DateTime.Now;

                if (Target != null) LogViewModel.Write("Now targeting " + Target.Name + " : " + Target.Id);
            }

            return false;
        }
    }
}