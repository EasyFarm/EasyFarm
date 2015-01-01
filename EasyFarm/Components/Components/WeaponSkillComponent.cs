
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

using EasyFarm.UserSettings;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroLimits.FarmingTool;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.Components
{
    /// <summary>
    /// Performs weaponskills on targets. 
    /// </summary>
    public class WeaponSkillComponent : MachineComponent
    {
        public FFACE FFACE { get; set; }

        public AbilityExecutor Executor { get; set; }

        public WeaponSkillComponent(FFACE fface)
        {
            this.FFACE = fface;
            this.Executor = new AbilityExecutor(fface);
        }

        public Unit Target
        {
            get { return AttackContainer.TargetUnit; }
            set { AttackContainer.TargetUnit = value; }
        }

        public override bool CheckComponent()
        {
            return Target != null && 
                FFACE.Player.Status.Equals(Status.Fighting) && 
                !Target.IsDead;
        }

        public override void RunComponent()
        {
            // Check engaged
            // FIXED: no longer return on not engage but don't execute 
            // these moves instead. Fixes the bot not attacking things 
            // from move than 30 yalms problem. 
            if (FFACE.Player.Status.Equals(Status.Fighting))
            {
                // Weaponskill
                if (ShouldWeaponSkill)
                {
                    // Not sure if weapon skills or job abilities endure the same penalties that 
                    // spell do in regards to wait times. So I'm using zero's here. 
                    AbilityExecutor.CastLatency = 0;
                    AbilityExecutor.GlobalCooldown = 0;

                    // Cast the weaponskill. 
                    this.Executor.UseAbility(Config.Instance.WeaponSkill.Ability);

                    // Rest casting parameters. 
                    AbilityExecutor.CastLatency = Config.Instance.MiscSettings.CastLatency;
                    AbilityExecutor.GlobalCooldown = Config.Instance.MiscSettings.GlobalCooldown;
                }
            }
        }

        /// <summary>
        /// Can we perform our weaponskill on the target unit?
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool ShouldWeaponSkill
        {
            get
            {
                return ActionFilters.WeaponSkillFilter(FFACE)
                    (Config.Instance.WeaponSkill, Target);
            }
        }
    }
}
