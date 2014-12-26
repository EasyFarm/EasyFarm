
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
    public class PullComponent : MachineComponent
    {
        public FFACE FFACE { get; set; }

        public AbilityExecutor Executor { get; set; }

        public Unit Target
        {
            get { return AttackContainer.TargetUnit; }
            set { AttackContainer.TargetUnit = value; }
        }

        public PullComponent(FFACE fface)
        {
            this.FFACE = fface;
            this.Executor = new AbilityExecutor(fface);
        }

        public override bool CheckComponent()
        {
            var TargetValid = Target != null && !Target.Status.Equals(Status.Fighting) && !Target.IsDead;

            return !AttackContainer.FightStarted && TargetValid;
        }

        public override void EnterComponent() 
        {
            FFACE.Navigator.Reset();
        }

        public override void RunComponent()
        {
            var Usable = Config.Instance.ActionInfo.PullList
                    .Where(x => x.Enabled && x.IsCastable(FFACE))
                    .Where(x => Target.Distance < x.Distance);

            // Only cast buffs when their status effects are not on the player. 
            var Buffs = Usable.Where(x => x.HasEffectWore(FFACE))
                .Select(x => x.Ability);

            // Cast the other abilities on cooldown. 
            var Others = Usable.Where(x => !x.HasEffectWore(FFACE))
                .Where(x => !x.IsBuff())
                .Select(x => x.Ability);

            // Recast the buffs. 
            this.Executor.EnsureSpellsCast(Buffs.ToList());

            // Recast others on cooldown. 
            this.Executor.EnsureSpellsCast(Others.ToList());
        }

        public override void ExitComponent()
        {
            if (Target.Status.Equals(Status.Fighting))
            {
                AttackContainer.FightStarted = true;
            }
        }
    }
}
