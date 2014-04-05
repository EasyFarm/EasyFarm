
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

﻿using EasyFarm.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EasyFarm
{
    partial class ViewModel
    {
        /// <summary>
        /// Gets target's name; textbox's text field.
        /// </summary>
        public String TargetsName
        {
            get { return Engine.Config.FilterInfo.TargetName; }
            set
            {
                Engine.Config.FilterInfo.TargetName = value;
                RaisePropertyChanged("TargetsName");
            }
        }

        /// <summary>
        /// Gets the list of targets; combo boxes items.
        /// </summary>
        public ObservableCollection<String> Targets
        {
            get { return Engine.Config.FilterInfo.TargetedMobs; }
            set
            {
                Engine.Config.FilterInfo.TargetedMobs = value;
            }
        }

        /// <summary>
        /// Gets the bool value that determines whether we should kill aggro; 
        /// Check box value.
        /// </summary>
        public bool KillAggro
        {
            get { return Engine.Config.FilterInfo.AggroFilter; }
            set
            {
                Engine.Config.FilterInfo.AggroFilter = value;
                RaisePropertyChanged("KillAggro");
            }
        }

        public bool KillUnclaimed
        {
            get { return Engine.Config.FilterInfo.UnclaimedFilter; }
            set
            {
                Engine.Config.FilterInfo.UnclaimedFilter = value;
                RaisePropertyChanged("KillUnclaimed");
            }
        }

        public bool KillPartyClaimed
        {
            get { return Engine.Config.FilterInfo.PartyFilter; }
            set
            {
                Engine.Config.FilterInfo.PartyFilter = value;
                RaisePropertyChanged("KillPartyClaimed");
            }
        }

        public ICommand AddTargetUnitCommand { get; set; }

        public ICommand DeleteTargetUnitCommand { get; set; }

        public ICommand ClearTargetUnitsCommand { get; set; }
    }
}