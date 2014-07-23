
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


using EasyFarm.GameData;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Windows.Input;
using ZeroLimits.FarmingTool;

namespace EasyFarm.ViewModels
{
    public class WeaponsViewModel : ViewModelBase
    {
        public WeaponsViewModel(FarmingTools farmingTools) : base(farmingTools) 
        {
            SetCommand = new DelegateCommand(SetWeaponSkill);
        }

        public String Name
        {
            get { return ftools.UserSettings.WeaponInfo.Name; }
            set { SetProperty(ref this.ftools.UserSettings.WeaponInfo.Name, value); }
        }

        public double Distance
        {
            get { return ftools.UserSettings.WeaponInfo.Distance; }
            set { SetProperty(ref this.ftools.UserSettings.WeaponInfo.Distance, value); 
                    App.InformUser("Distance set to " + this.Distance);
            }
        }

        public int Health
        {
            get { return ftools.UserSettings.WeaponInfo.Health; }
            set
            {
                SetProperty(ref this.ftools.UserSettings.WeaponInfo.Health, value); 
                App.InformUser("Health set to " + this.Health);
            }
        }

        public WeaponAbility Ability
        {
            get { return ftools.UserSettings.WeaponInfo; }
            set { SetProperty(ref this.ftools.UserSettings.WeaponInfo, value); }
        }

        public ICommand SetCommand { get; set; }

        private void SetWeaponSkill()
        {
            var weaponSkill = ftools.AbilityService.CreateAbility(Name);

            if (string.IsNullOrWhiteSpace(Name) || !weaponSkill.IsValidName)
            {
                App.InformUser("Failed to set weaponskill.");
                return;
            }

            this.Ability.Ability = weaponSkill;
            this.Ability.Distance = Distance;
            this.Ability.Health = Health;
            this.Ability.Enabled = true;
            this.Ability.Name = Name;

            App.InformUser("Weaponskill set.");
        }
    }
}