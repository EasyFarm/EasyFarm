
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
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
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
        public WeaponsViewModel(ref GameEngine Engine, IEventAggregator eventAggregator) :
            base(ref Engine, eventAggregator) 
        {
            SetCommand = new DelegateCommand<Object>(SetWeaponSkill);
        }

        public String Name
        {
            get { return _engine.UserSettings.WeaponInfo.Name; }
            set { SetProperty(ref this._engine.UserSettings.WeaponInfo.Name, value); }
        }

        public double Distance
        {
            get { return _engine.UserSettings.WeaponInfo.Distance; }
            set { SetProperty(ref this._engine.UserSettings.WeaponInfo.Distance, value); 
                    InformUser("Distance set to " + this.Distance);
            }
        }

        public int Health
        {
            get { return _engine.UserSettings.WeaponInfo.Health; }
            set
            {
                SetProperty(ref this._engine.UserSettings.WeaponInfo.Health, value); 
                InformUser("Health set to " + this.Health);
            }
        }

        public WeaponAbility Ability
        {
            get { return _engine.UserSettings.WeaponInfo; }
            set { SetProperty(ref this._engine.UserSettings.WeaponInfo, value); }
        }

        public ICommand SetCommand { get; set; }

        private void SetWeaponSkill(Object StatusBar)
        {
            var weaponSkill = _engine.AbilityService.CreateAbility(Name);

            if (string.IsNullOrWhiteSpace(Name) || !weaponSkill.IsValidName)
            {
                InformUser("Failed to set weaponskill.");
                return;
            }

            this.Ability.Ability = weaponSkill;
            this.Ability.Distance = Distance;
            this.Ability.Health = Health;
            this.Ability.Enabled = true;
            this.Ability.Name = Name;

            InformUser("Weaponskill set.");
        }
    }
}