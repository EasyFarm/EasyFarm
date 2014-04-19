
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EasyFarm.ViewModels
{
    public class HealingViewModel : ViewModelBase
    {
        public HealingViewModel(ref GameEngine Engine)
            : base(ref Engine)
        {
            AddHealingCommand = new RelayCommand(AddHealingItem);
            DeleteHealingCommand = new RelayCommand<Object>(DeleteHealing);
            ClearHealingCommand = new RelayCommand(ClearHealing);
        }

        public ObservableCollection<ListItem<HealingAbility>> Healing
        {
            get { return GameEngine.Config.ActionInfo.HealingList; }
            set
            {
                GameEngine.Config.ActionInfo.HealingList = value;
                this.RaisePropertyChanged("Healing");
            }
        }

        public ICommand AddHealingCommand { get; set; }

        public ICommand DeleteHealingCommand { get; set; }

        public ICommand ClearHealingCommand { get; set; }

        private void ClearHealing()
        {
            Healing.Clear();
        }

        private void DeleteHealing(object obj)
        {
            Healing.Remove(obj as ListItem<HealingAbility>);
        }

        private void AddHealingItem()
        {
            Healing.Add(new ListItem<HealingAbility>(new HealingAbility()));
        }
    }
}
