
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
using EasyFarm.Logging;
using EasyFarm.UserSettings;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroLimits.FarmingTool;
using ZeroLimits.XITool;
using ZeroLimits.XITool.Classes;


namespace EasyFarm.Components
{
    /// <summary>
    /// Buffs the player. 
    /// </summary>
    public class BuffComponent : MachineComponent
    {
        public FFACE FFACE { get; set; }

        public AbilityExecutor Executor { get; set; }

        public BuffComponent(FFACE fface)
        {
            this.FFACE = fface;
            this.Executor = new AbilityExecutor(fface);
        }

        public override bool CheckComponent()
        {
            return (Target != null && !AttackContainer.FightStarted && !Target.IsDead);
        }

        public override void EnterComponent() { }

        public override void RunComponent()
        {
            var Usable = Config.Instance.ActionInfo.StartList
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
        }

        public override void ExitComponent() { }

        public Unit Target
        {
            get { return AttackContainer.TargetUnit; }
            set { AttackContainer.TargetUnit = value; }
        }
    }
}
