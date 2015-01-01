
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

namespace EasyFarm.Components
{
    public class PostBattleComponent : BaseComponent
    {
        public AbilityExecutor Executor { get; set; }

        public UnitService Units { get; set; }

        public Unit Target
        {
            get { return AttackContainer.TargetUnit; }
            set { AttackContainer.TargetUnit = value; }
        }

        public PostBattleComponent(FFACE fface)
            : base(fface)
        {
            this.Executor = new AbilityExecutor(fface);
            this.Units = new UnitService(fface);

            // Set default filter to target mobs. 
            this.Units.UnitFilter = UnitFilters.MobFilter(fface);
        }

        public override bool CheckComponent()
        {
            return ((Target == null || Target.IsDead));
        }

        public override void RunComponent()
        {
            var Usable = Config.Instance.ActionInfo.EndList
                    .Where(x => x.Enabled && x.IsCastable(FFACE));

            // Only cast buffs when their status effects are not on the player. 
            var Buffs = Usable
                .Where(x => x.HasEffectWore(FFACE))
                .Select(x => x.Ability);

            // Cast the other abilities on cooldown. 
            var Others = Usable.Where(x => !x.HasEffectWore(FFACE))
                .Where(x => !x.IsBuff())
                .Select(x => x.Ability);

            // Recast the buffs. 
            this.Executor.EnsureSpellsCast(Buffs.ToList());

            // Recast others on cooldown. 
            this.Executor.EnsureSpellsCast(Others.ToList());

            // Get the next target by distance to the player. 
            var Target = ftools.UnitService.GetTarget(
                UnitFilters.MobFilter(FFACE), x => x.Distance);

            // Set our new target at the end so that we don't accidentaly cast on a 
            // new target. 
            AttackContainer.TargetUnit = Target;

            // Set to false in order to use starting moves again in the 
            // attack Component. 
            AttackContainer.FightStarted = false;

            App.Current.Dispatcher.Invoke(() =>
            {
                if (Target != null)
                {
                    Logger.Write.StateRun("Now targeting " + Target.Name + " : " + Target.ID);
                }
            });
        }
    }
}
