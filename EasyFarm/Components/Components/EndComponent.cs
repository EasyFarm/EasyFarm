
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

using EasyFarm.FarmingTool;
using FFACETools;
using System;
using ZeroLimits.FarmingTool;
using ZeroLimits.XITool.Classes;
using System.Linq;
using EasyFarm.ViewModels;
using EasyFarm.UserSettings;
using EasyFarm.Logging;
using EasyFarm.Classes;

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
            var Usable = Config.Instance.EndList
                    .Where(x => x.Enabled && x.IsCastable(FFACE));

            // Only cast buffs when their status effects are not on the player. 
            var Buffs = Usable.Where(x => x.HasEffectWore(FFACE));

            // Cast the other abilities on cooldown. 
            var Others = Usable.Where(x => !x.HasEffectWore(FFACE))
                .Where(x => !x.IsBuff());

            // Execute moves. 
            Executor.ExecuteBuffs(Buffs.Union(Others));

            // Get mobs sorted first by party claim, then aggro 
            // and finally distance. 
            var target = Units.GetUnits(UnitFilters.MobFilter(FFACE))
                .OrderBy(x => x.PartyClaim)
                .ThenBy(x => x.HasAggroed)
                .ThenBy(x => x.Distance)
                .FirstOrDefault();

            // Set our new target at the end so that we don't accidentaly cast on a 
            // new target. 
            AttackContainer.TargetUnit = target;

            if (App.Current != null)
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    if (Target != null)
                    {
                        Logger.Write.StateRun("Now targeting " + Target.Name + " : " + Target.ID);
                    }
                });
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
