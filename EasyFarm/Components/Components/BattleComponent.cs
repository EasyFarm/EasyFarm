
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

using FFACETools;
using System.Threading;
using ZeroLimits.FarmingTool;
using ZeroLimits.XITool;
using System.Linq;
using System.Collections.Generic;
using System;
using ZeroLimits.XITool.Classes;
using EasyFarm.ViewModels;
using EasyFarm.UserSettings;
using EasyFarm.Logging;
using EasyFarm.FarmingTool;
using EasyFarm.Classes;

namespace EasyFarm.Components
{
    /// <summary>
    /// A class for defeating monsters.
    /// </summary>
    public class BattleComponent : MachineComponent
    {
        public FFACE FFACE { get; set; }

        public RestingService Resting { get; set; }

        public Executor Executor { get; set; }

        /// <summary>
        /// Who we are trying to kill currently
        /// </summary>
        public Unit Target
        {
            get { return AttackContainer.TargetUnit; }
            set { AttackContainer.TargetUnit = value; }
        }

        public BattleComponent(FFACE fface)
        {
            this.FFACE = fface;
            this.Resting = new RestingService(fface);
            this.Executor = new Executor(fface);
        }

        public override bool CheckComponent()
        {
            // target null or dead. 
            if (Target == null || Target.IsDead || Target.ID == 0) return false;

            // Mobs has not been pulled if pulling moves are available. 
            if (!AttackContainer.FightStarted) return false;

            // Engage is enabled and we are not engaged. We cannot proceed. 
            if (Config.Instance.IsEngageEnabled)
                return FFACE.Player.Status.Equals(Status.Fighting);
            // Engage is not checked, so just proceed to battle. 
            else return true;
        }

        public override void EnterComponent()
        {
            Resting.EndResting();
            FFACE.Navigator.Reset();
        }

        public override void RunComponent()
        {
            var Usable = Config.Instance.BattleList
                .Where(x => x.Enabled && x.IsCastable(FFACE));

            var Buffs = Usable.Where(x => x.HasEffectWore(FFACE));

            var Others = Usable.Where(x => !x.HasEffectWore(FFACE))
                .Where(x => !x.IsBuff());

            // Execute moves at target. 
            Executor.Target = Target;
            Executor.ExecuteActions(Buffs.Union(Others));
        }
    }
}
