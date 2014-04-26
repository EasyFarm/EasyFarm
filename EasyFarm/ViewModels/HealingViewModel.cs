
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EasyFarm.ViewModels
{
    public class HealingViewModel : ViewModelBase
    {
        public HealingViewModel(ref GameEngine Engine, IEventAggregator eventAggregator)
            : base(ref Engine, eventAggregator)
        {
            AddHealingCommand = new DelegateCommand(AddHealingItem);
            DeleteHealingCommand = new DelegateCommand<Object>(DeleteHealing);
            ClearHealingCommand = new DelegateCommand(ClearHealing);
        }

        public ObservableCollection<ListItem<HealingAbility>> Healing
        {
            get { return GameEngine.UserSettings.ActionInfo.HealingList; }
            set { SetProperty(ref this.GameEngine.UserSettings.ActionInfo.HealingList, value); }
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
