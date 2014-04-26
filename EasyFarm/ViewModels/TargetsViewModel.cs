
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
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EasyFarm.ViewModels
{
    public class TargetsViewModel : ViewModelBase
    {
        public TargetsViewModel(ref GameEngine Engine, IEventAggregator eventAggregator) : 
            base(ref Engine, eventAggregator) 
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
            if(!Targets.Contains(TargetsName))
                Targets.Add(TargetsName);
        }

        public String TargetsName
        {
            get { return GameEngine.UserSettings.FilterInfo.TargetName; }
            set { SetProperty(ref this.GameEngine.UserSettings.FilterInfo.TargetName, value); }
        }

        public ObservableCollection<String> Targets
        {
            get { return GameEngine.UserSettings.FilterInfo.TargetedMobs; }
            set { SetProperty(ref this.GameEngine.UserSettings.FilterInfo.TargetedMobs, value); }
        }

        public bool Aggro
        {
            get { return GameEngine.UserSettings.FilterInfo.AggroFilter; }
            set { SetProperty(ref this.GameEngine.UserSettings.FilterInfo.AggroFilter, value); }
        }

        public bool Unclaimed
        {
            get { return GameEngine.UserSettings.FilterInfo.UnclaimedFilter; }
            set { SetProperty(ref this.GameEngine.UserSettings.FilterInfo.UnclaimedFilter, value); }
        }

        public bool PartyClaimed
        {
            get { return GameEngine.UserSettings.FilterInfo.PartyFilter; }
            set { SetProperty(ref this.GameEngine.UserSettings.FilterInfo.PartyFilter, value); }
        }

        public ICommand AddCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand ClearCommand { get; set; }
    }
}