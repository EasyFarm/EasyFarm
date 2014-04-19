
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013 - 2014>  <Zerolimits>

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

using EasyFarm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    public class WeaponInfo
    {
        AbilityService _abilityService;

        public WeaponInfo()
        {
            _abilityService = new AbilityService();
            SetDefaults();
        }

        private void SetDefaults()
        {
            this.WeaponSkill = new WeaponAbility();
            HealthThreshold = 0;
            Name = "";
            Distance = 0;
        }

        /// <summary>
        /// The weaponskill that should be used when we reach 100% tp
        /// </summary>
        public WeaponAbility WeaponSkill 
        { 
            get;
            set; 
        }

        /// <summary>
        /// Tells us when to use the weaponskill when the mob's hp reaches this level
        /// </summary>
        public int HealthThreshold 
        {
            get { return WeaponSkill.HPTrigger; }
            set { WeaponSkill.HPTrigger = value; }
        }

        /// <summary>
        /// The name of the weaponskill that will be created
        /// </summary>
        public string Name 
        {
            get { return WeaponSkill.Name; }
            set { WeaponSkill.Name = value; }
        }

        /// <summary>
        /// The max distance the weaponskill should be used at
        /// </summary>
        public double Distance 
        {
            get { return WeaponSkill.DistanceTrigger; }
            set { WeaponSkill.DistanceTrigger = value; }
        }
    }
}
