
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
using EasyFarm.UserSettings;
using FFACETools;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.Components
{
    public class PullComponent : MachineComponent
    {
        public FFACE FFACE { get; set; }

        public Executor Executor { get; set; }

        public Unit Target
        {
            get { return AttackContainer.TargetUnit; }
            set { AttackContainer.TargetUnit = value; }
        }        

        public PullComponent(FFACE fface)
        {
            this.FFACE = fface;
            this.Executor = new Executor(fface);           
        }

        public override bool CheckComponent()
        {
            // Invalid target: dead or null
            if (Target == null || Target.IsDead || Target.ID == 0) return false;

            // We've already tried to pull once, cancel. 
            if(AttackContainer.FightStarted) return false;

            // We've succeeded at pulling the mob!
            return !Target.Status.Equals(Status.Fighting);
        }

        public override void EnterComponent() 
        {
            FFACE.Navigator.Reset();
        }

        public override void RunComponent()
        {
            var Usable = Config.Instance.PullList
                    .Where(x => x.Enabled && x.IsCastable(FFACE));

            // Only cast buffs when their status effects are not on the player. 
            var Buffs = Usable.Where(x => x.HasEffectWore(FFACE));

            // Cast the other abilities on cooldown. 
            var Others = Usable.Where(x => !x.HasEffectWore(FFACE))
                .Where(x => !x.IsBuff());

            // Execute all abilities. 
            Executor.Target = Target;
            Executor.ExecuteActions(Buffs.Union(Others));
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
