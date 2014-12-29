
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

using EasyFarm.GameData;
using EasyFarm.UserSettings;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Windows.Input;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.ViewModels
{
    [ViewModelAttribute("WeaponSkill")]
    public class WeaponsViewModel : ViewModelBase
    {
        AbilityService AbilityService = new AbilityService();

        public WeaponsViewModel() 
        {
            SetCommand = new DelegateCommand(SetWeaponSkill);
        }

        public String Name
        {
            get { return Config.Instance.WeaponSkill.Name; }
            set { SetProperty(ref Config.Instance.WeaponSkill.Name, value); }
        }

        public double Distance
        {
            get { return Config.Instance.WeaponSkill.Distance; }
            set
            {
                SetProperty(ref Config.Instance.WeaponSkill.Distance, value);
                InformUser("Distance set to " + Distance);
            }
        }

        public int UpperHealth
        {
            get { return Config.Instance.WeaponSkill.UpperHealth; }
            set
            {
                SetProperty(ref Config.Instance.WeaponSkill.UpperHealth, value);
                InformUser("Upper health set to " + UpperHealth);
            }
        }

        public int LowerHealth
        {
            get { return Config.Instance.WeaponSkill.LowerHealth; }
            set
            {
                SetProperty(ref Config.Instance.WeaponSkill.LowerHealth, value);
                InformUser("Lower health set to " + LowerHealth);
            }
        }

        public WeaponSkill WeaponSkill
        {
            get { return Config.Instance.WeaponSkill; }
            set { SetProperty(ref Config.Instance.WeaponSkill, value); }
        }

        public ICommand SetCommand { get; set; }

        private void SetWeaponSkill()
        {
            var weaponSkill = AbilityService.CreateAbility(Name);

            if (string.IsNullOrWhiteSpace(Name) || !weaponSkill.IsValidName)
            {
                InformUser("Failed to set weaponskill.");
                return;
            }

            WeaponSkill.Ability = weaponSkill;
            WeaponSkill.Distance = Distance;
            WeaponSkill.UpperHealth = UpperHealth;
            WeaponSkill.LowerHealth = LowerHealth;
            WeaponSkill.Enabled = true;
            WeaponSkill.Name = Name;

            InformUser("Weaponskill set.");
        }
    }
}