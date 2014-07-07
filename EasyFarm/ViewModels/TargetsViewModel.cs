
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
using EasyFarm.Classes.Services;
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
        public TargetsViewModel(FarmingTools farmingTools) : base(farmingTools) 
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
            get { return farmingTools.UnitService.FilterInfo.TargetName; }
            set { SetProperty(ref farmingTools.UnitService.FilterInfo.TargetName, value); }
        }

        public ObservableCollection<String> Targets
        {
            get { return farmingTools.UnitService.FilterInfo.TargetedMobs; }
            set { SetProperty(ref farmingTools.UnitService.FilterInfo.TargetedMobs, value); }
        }

        public bool Aggro
        {
            get { return farmingTools.UnitService.FilterInfo.AggroFilter; }
            set { SetProperty(ref farmingTools.UnitService.FilterInfo.AggroFilter, value); }
        }

        public bool Unclaimed
        {
            get { return farmingTools.UnitService.FilterInfo.UnclaimedFilter; }
            set { SetProperty(ref farmingTools.UnitService.FilterInfo.UnclaimedFilter, value); }
        }

        public bool PartyClaimed
        {
            get { return farmingTools.UnitService.FilterInfo.PartyFilter; }
            set { SetProperty(ref farmingTools.UnitService.FilterInfo.PartyFilter, value); }
        }

        public ICommand AddCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand ClearCommand { get; set; }
    }
}