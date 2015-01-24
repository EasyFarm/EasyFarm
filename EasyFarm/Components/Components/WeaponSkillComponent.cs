
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

        public Caster Caster { get; set; }

        public WeaponSkillComponent(FFACE fface)
        {
            this.FFACE = fface;
            this.Caster = new Caster(fface);
        }

        public Unit Target
        {
            get { return AttackContainer.TargetUnit; }
            set { AttackContainer.TargetUnit = value; }
        }

        public override bool CheckComponent()
        {
            // target dead or null
            if (Target == null || Target.IsDead) return false;

            // Use skill if we are engaged. 
            return (FFACE.Player.Status.Equals(Status.Fighting));
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
                    // Cast the weaponskill. 
                    this.Caster.CastAbility(Config.Instance.WeaponSkill.Ability);
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
