
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

using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZeroLimits.FarmingTool;


namespace EasyFarm.ViewModels
{
    public class TargetsViewModel : ViewModelBase
    {
        public TargetsViewModel(FarmingTools farmingTools)
            : base(farmingTools)
        {
            this.AddCommand = new DelegateCommand(AddTargetCommand);
            this.DeleteCommand = new DelegateCommand(DeleteTargetCommand);
            this.ClearCommand = new DelegateCommand(ClearTargetsCommand);
        }

        private void ClearTargetsCommand()
        {
            Targets.Clear();
        }

        private void DeleteTargetCommand()
        {
            if (Targets.Contains(TargetsName))
                Targets.Remove(TargetsName);
        }

        private void AddTargetCommand()
        {
            if (!Targets.Contains(TargetsName))
                Targets.Add(TargetsName);
        }

        public String TargetsName
        {
            get { return ftools.UserSettings.FilterInfo.TargetName; }
            set { SetProperty(ref ftools.UserSettings.FilterInfo.TargetName, value); }
        }

        public ObservableCollection<String> Targets
        {
            get { return ftools.UserSettings.FilterInfo.TargetedMobs; }
            set { SetProperty(ref ftools.UserSettings.FilterInfo.TargetedMobs, value); }
        }

        public bool Aggro
        {
            get { return ftools.UserSettings.FilterInfo.AggroFilter; }
            set { SetProperty(ref ftools.UserSettings.FilterInfo.AggroFilter, value); }
        }

        public bool Unclaimed
        {
            get { return ftools.UserSettings.FilterInfo.UnclaimedFilter; }
            set { SetProperty(ref ftools.UserSettings.FilterInfo.UnclaimedFilter, value); }
        }

        public bool PartyClaimed
        {
            get { return ftools.UserSettings.FilterInfo.PartyFilter; }
            set { SetProperty(ref ftools.UserSettings.FilterInfo.PartyFilter, value); }
        }

        public bool Claimed
        {
            get { return ftools.UserSettings.FilterInfo.ClaimedFilter; }
            set { SetProperty(ref ftools.UserSettings.FilterInfo.ClaimedFilter, value); }
        }

        public ICommand AddCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand ClearCommand { get; set; }
    }
}