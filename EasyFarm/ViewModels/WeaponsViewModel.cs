
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

using EasyFarm.Classes;
using MvvmFoundation.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace EasyFarm.ViewModels
{
    public class WeaponsViewModel : ViewModelBase
    {
        public WeaponsViewModel(ref GameEngine Engine)
            : base(ref Engine)
        {
            SetCommand = new RelayCommand<Object>(SetWeaponSkill);
        }

        public String Name
        {
            get { return GameEngine.Config.WeaponInfo.Name; }
            set
            {
                GameEngine.Config.WeaponInfo.Name = value;
                this.RaisePropertyChanged("Name");
            }
        }

        public double Distance
        {
            get { return GameEngine.Config.WeaponInfo.Distance; }
            set
            {
                GameEngine.Config.WeaponInfo.Distance = value;
                this.RaisePropertyChanged("Distance");
            }
        }

        public int Health
        {
            get { return GameEngine.Config.WeaponInfo.HealthThreshold; }
            set
            {
                GameEngine.Config.WeaponInfo.HealthThreshold = value;
                this.RaisePropertyChanged("Health");
            }
        }

        public WeaponAbility Skill
        {
            get { return GameEngine.Config.WeaponInfo.WeaponSkill; }
            set
            {
                this.GameEngine.Config.WeaponInfo.WeaponSkill = value;
                this.RaisePropertyChanged("Skill");
            }
        }

        public ICommand SetCommand { get; set; }

        private void SetWeaponSkill(Object StatusBar)
        {
            WeaponAbility WeaponSkill = 
                new WeaponAbility(GameEngine.AbilityService.CreateAbility(Name));

            if (string.IsNullOrWhiteSpace(Name) || !WeaponSkill.Ability.IsValidName)
            {
                // (StatusBar as Label).Content = "Failed to set weaponskill.";
                return;
            }

            WeaponSkill.DistanceTrigger = Distance;
            WeaponSkill.HPTrigger = Health;
            WeaponSkill.IsEnabled = true;
            WeaponSkill.Name = Name;

            GameEngine.Config.WeaponInfo.WeaponSkill = WeaponSkill;
        }
    }
}