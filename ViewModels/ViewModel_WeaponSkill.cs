
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
*////////////////////////////////////////////////////////////////////

﻿using EasyFarm.Classes;
using EasyFarm.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EasyFarm
{
    public partial class ViewModel
    {
        public String WeaponSkillName
        {
            get { return Engine.Config.WeaponInfo.Name; }
            set 
            {
                Engine.Config.WeaponInfo.Name = value;
                RaisePropertyChanged("WeaponSkillName");
            }
        }

        public double WeaponSkillDistance
        {
            get { return Engine.Config.WeaponInfo.Distance; }
            set
            {
                Engine.Config.WeaponInfo.Distance = value;
                StatusBarText = "Distance: " + value;
                RaisePropertyChanged("WeaponSkillDistance");
            }
        }

        public int WeaponSkillHealth
        {
            get { return Engine.Config.WeaponInfo.HealthThreshold; }
            set 
            {
                Engine.Config.WeaponInfo.HealthThreshold = (int)value;
                StatusBarText = "Health: " + (int)value;
                RaisePropertyChanged("WeaponSkillHealth");
            }
        }

        public ICommand SetWeaponSkillCommand { get; set; }

        private void SetWeaponSkill()
        {
            if (!string.IsNullOrWhiteSpace(Engine.Config.WeaponInfo.Name))
            {
                var Weaponskill = new WeaponAbility(WeaponSkillName);
             
                if (Weaponskill.Ability.IsValidName)
                {
                    Weaponskill.DistanceTrigger = WeaponSkillDistance;
                    Weaponskill.HPTrigger = WeaponSkillHealth;
                    Weaponskill.Name = WeaponSkillName;
                    Engine.Config.WeaponInfo.WeaponSkill = Weaponskill;
                    StatusBarText = Weaponskill.Name + " : now set!";
                }
                else 
                {
                    StatusBarText = "Failed to add weaponskill";
                }
            }
        }
    }
}