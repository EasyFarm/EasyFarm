
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EasyFarm
{
    partial class ViewModel
    {
        public ObservableCollection<ListItem<HealingAbility>> HealingList
        {
            get { return Engine.Config.ActionInfo.HealingList; }
            set { Engine.Config.ActionInfo.HealingList = value; }
        }

        public ICommand AddHealingCommand { get; set; }

        public ICommand DeleteHealingCommand { get; set; }

        public ICommand ClearHealingCommand { get; set; }

        private void ClearHealing()
        {
            HealingList.Clear();
        }

        private void DeleteHealing(object obj)
        {
            HealingList.Remove(obj as ListItem<HealingAbility>);
        }

        private void AddHealingItem()
        {
            HealingList.Add(new ListItem<HealingAbility>(new HealingAbility() { IsEnabled = false, Name = "Empty", TriggerLevel = 0}));
        }
    }
}
