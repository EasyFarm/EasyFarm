
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
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZeroLimits.FarmingTool;


namespace EasyFarm.ViewModels
{
    public class HealingViewModel : ViewModelBase
    {
        public HealingViewModel() 
        {
            AddHealingCommand = new DelegateCommand(AddHealingItem);
            DeleteHealingCommand = new DelegateCommand<Object>(DeleteHealing);
            ClearHealingCommand = new DelegateCommand(ClearHealing);
            EnsureOneHealingAbility();
        }

        public ObservableCollection<HealingAbility> Healing
        {
            get { return Config.Instance.ActionInfo.HealingList; }
            set { SetProperty(ref Config.Instance.ActionInfo.HealingList, value); }
        }

        public ICommand AddHealingCommand { get; set; }

        public ICommand DeleteHealingCommand { get; set; }

        public ICommand ClearHealingCommand { get; set; }

        private void ClearHealing()
        {
            // Clear all current items.
            Healing.Clear();
            
            // Leave on so that the interface looks better and
            // it makes it easier for the user to add one. 
            EnsureOneHealingAbility();
        }

        private void DeleteHealing(object obj)
        {
            Healing.Remove(obj as HealingAbility);
            EnsureOneHealingAbility();
        }

        private void AddHealingItem()
        {
            Healing.Add(new HealingAbility());
        }

        /// <summary>
        /// Adds a healing ability to healing if none is present. 
        /// </summary>
        private void EnsureOneHealingAbility()
        {
            if (Healing.Count <= 0)
            {
                Healing.Add(new HealingAbility());
            }
        }
    }
}
