
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

using EasyFarm.Classes;
using EasyFarm.Logging;
using EasyFarm.UserSettings;
using FFACETools;
using System;
using System.Linq;

namespace EasyFarm.Components
{
    /// <summary>
    /// Handles the end of battle situation. 
    /// Fires off the end list, sets FightStart to true so other 
    /// lists can fire and replaces targets that are dead, null, 
    /// empty or invalid. 
    /// </summary>
    public class EndComponent : BaseComponent
    {
        public Executor Executor { get; set; }

        public UnitService Units { get; set; }

        private DateTime lastCheckedForMob = DateTime.Now;

        public Unit Target
        {
            get { return AttackContainer.TargetUnit; }
            set { AttackContainer.TargetUnit = value; }
        }

        public EndComponent(FFACE fface)
            : base(fface)
        {
            this.Executor = new Executor(fface);
            this.Units = new UnitService(fface);

            // Set default filter to target mobs. 
            this.Units.UnitFilter = UnitFilters.MobFilter(fface);
        }

        public override bool CheckComponent()
        {
            // Null, dead and empty mob check. 
            if ((Target == null || Target.IsDead || Target.ID == 0)) return true;

            // Creature is unkillable and does not meets the 
            // user's criteria for valid mobs defined in MobFilters. 
            return !Units.IsValid(Target);
        }

        public override void RunComponent()
        {
            // Execute moves. 
            var Usable = Config.Instance.BattleLists["End"].Actions
                .Where(x => ActionFilters.BattleAbilityFilter(FFACE, x));

            Executor.UseBuffingActions(Usable);

            if (lastCheckedForMob.AddSeconds(Constants.UNIT_ARRAY_CHECK_RATE) < DateTime.Now)
            {
                // First get the first mob by distance. 
                var mobs = Units.MOBArray.Where(x => Units.IsValid(x))
                    .OrderByDescending(x => x.PartyClaim)
                    .ThenByDescending(x => x.HasAggroed)
                    .ThenBy(x => x.Distance)
                    .ToList();

                // Set our new target at the end so that we don't accidentaly cast on a 
                // new target. 
                AttackContainer.TargetUnit = mobs.FirstOrDefault();
            }

            if (Target != null)
            {
                Logger.Write.StateRun("Now targeting " + Target.Name + " : " + Target.ID);
            }
        }

        public override void ExitComponent()
        {
            // Set to false in order to use starting moves again in the 
            // attack Component. 
            AttackContainer.FightStarted = false;
        }
    }
}
