
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

namespace EasyFarm.Components
{
    /// <summary>
    /// A class for defeating monsters. 
    /// </summary>
    public class AttackContainer : SequenceContainer
    {
        public static bool FightStarted { get; set; }

        private static Unit m_targetUnit { get; set; }

        public FFACE FFACE { get; set; }

        static AttackContainer()
        {
            FightStarted = false;
            m_targetUnit = Unit.CreateUnit(0);
        }

        public AttackContainer(FFACE fface)
        {
            this.FFACE = fface;

            // Add components.
            this.AddComponent(new ApproachComponent(fface) { Priority = 0 });
            this.AddComponent(new BattleComponent(fface) { Priority = 3 });
            this.AddComponent(new WeaponSkillComponent(fface) { Priority = 2 });
            this.AddComponent(new PullComponent(fface) { Priority = 4 });
            this.AddComponent(new StartComponent(fface) { Priority = 5 });

            // Enable all attack components. 
            this.Components.ForEach(x => x.Enabled = true);
        }

        public override bool CheckComponent()
        {
            // If we're injured. 
            if (new RestComponent(FFACE).CheckComponent()) return false;

            if (new HealingComponent(FFACE).CheckComponent()) return false;

            // Return if other components need to fire. 
            return base.CheckComponent();
        }

        /// <summary>
        /// Who we are trying to kill currently
        /// </summary>
        public static Unit TargetUnit
        {
            get { return m_targetUnit; }
            set { m_targetUnit = value; }
        }
    }
}