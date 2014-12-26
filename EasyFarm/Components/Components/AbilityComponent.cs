
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

namespace EasyFarm.Components
{
    /// <summary>
    /// A class for defeating monsters.
    /// </summary>
    public class AbilityComponent : MachineComponent
    {
        public FFACE FFACE { get; set; }

        public UnitService Units { get; set; }

        public RestingService Resting { get; set; }

        public AbilityExecutor Executor { get; set; }

        /// <summary>
        /// Who we are trying to kill currently
        /// </summary>
        public Unit Target
        {
            get { return AttackContainer.TargetUnit; }
            set { AttackContainer.TargetUnit = value; }
        }

        public AbilityComponent(FFACE fface)
        {
            this.FFACE = fface;
            this.Units = new UnitService(fface);
            this.Resting = new RestingService(fface);
            this.Executor = new AbilityExecutor(fface);

            // Set default filter to target mobs. 
            this.Units.UnitFilter = UnitFilters.MobFilter(fface);
        }

        public override bool CheckComponent()
        {
            bool success = false;

            // If we have a valid target
            if (Units.IsValid(Target))
            {
                // If we're alive
                if (!FFACE.Player.Status.Equals(Status.Dead1 | Status.Dead2))
                {
                    // If we're not injured
                    if (!new RestComponent(FFACE).CheckComponent())
                        success = true;
                }
            }

            return success;
        }

        public override void EnterComponent()
        {
            Resting.EndResting();
            FFACE.Navigator.Reset();
        }

        public override void RunComponent()
        {
            ///////////////////////////////////////////////////////////////////
            // Battle Enemy. 
            ///////////////////////////////////////////////////////////////////

            // Check engaged
            // FIXED: no longer return on not engage but don't execute 
            // these moves instead. Fixes the bot not attacking things 
            // from move than 30 yalms problem. 
            if (FFACE.Player.Status.Equals(Status.Fighting))
            {
                var Usable = Config.Instance.ActionInfo.BattleList
                    .Where(x => x.Enabled && x.IsCastable(FFACE))
                    .Where(x => Target.Distance < x.Distance);

                var Buffs = Usable.Where(x => x.HasEffectWore(FFACE))
                    .Select(x => x.Ability);

                var Others = Usable.Where(x => !x.HasEffectWore(FFACE))
                    .Where(x => !x.IsBuff())
                    .Select(x => x.Ability);

                var move = Buffs.Union(Others).FirstOrDefault();
                
                if (move != null) this.Executor.UseAbility(move);

                // Cast buffs when worn off. 
                // this.Executor.ExecuteActions(Buffs.ToList());

                // Cast other moves on cooldown. 
                // this.Executor.ExecuteActions(Others.ToList());
            }
        }

        public override void ExitComponent() { }
    }
}
