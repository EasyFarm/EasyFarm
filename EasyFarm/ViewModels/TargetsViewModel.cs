
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

using EasyFarm.UserSettings;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZeroLimits.FarmingTool;


namespace EasyFarm.ViewModels
{
    public class TargetsViewModel : ViewModelBase
    {
        public TargetsViewModel()
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
            get { return Config.Instance.FilterInfo.TargetName; }
            set { SetProperty(ref Config.Instance.FilterInfo.TargetName, value); }
        }

        public ObservableCollection<String> Targets
        {
            get { return Config.Instance.FilterInfo.TargetedMobs; }
            set { SetProperty(ref Config.Instance.FilterInfo.TargetedMobs, value); }
        }

        public bool Aggro
        {
            get { return Config.Instance.FilterInfo.AggroFilter; }
            set { SetProperty(ref Config.Instance.FilterInfo.AggroFilter, value); }
        }

        public bool Unclaimed
        {
            get { return Config.Instance.FilterInfo.UnclaimedFilter; }
            set { SetProperty(ref Config.Instance.FilterInfo.UnclaimedFilter, value); }
        }

        public bool PartyClaimed
        {
            get { return Config.Instance.FilterInfo.PartyFilter; }
            set { SetProperty(ref Config.Instance.FilterInfo.PartyFilter, value); }
        }

        public bool Claimed
        {
            get { return Config.Instance.FilterInfo.ClaimedFilter; }
            set { SetProperty(ref Config.Instance.FilterInfo.ClaimedFilter, value); }
        }

        public bool BitCheck
        {
            get { return Config.Instance.FilterInfo.BitCheck; }
            set { SetProperty(ref Config.Instance.FilterInfo.BitCheck, value); }
        }

        public ICommand AddCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand ClearCommand { get; set; }
    }
}