
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
    [ViewModelAttribute("Ignored")]
    public class IgnoredViewModel : ViewModelBase
    {
        public IgnoredViewModel() 
        {
            AddIgnoredUnitCommand = new DelegateCommand(AddIgnoredUnit);
            DeleteIgnoredUnitCommand = new DelegateCommand(DeleteIgnoredUnit);
            ClearIgnoredUnitsCommand = new DelegateCommand(ClearIgnoredUnits);
        }

        private void ClearIgnoredUnits()
        {
            Ignored.Clear();
        }

        private void DeleteIgnoredUnit()
        {
            if (Ignored.Contains(Name))
                Ignored.Remove(Name);
        }

        private void AddIgnoredUnit()
        {
            if(!Ignored.Contains(Name))
                Ignored.Add(Name);
        }

        public String Name
        {
            get { return Config.Instance.FilterInfo.IgnoredName; }
            set { SetProperty(ref Config.Instance.FilterInfo.IgnoredName, value); }
        }

        public ObservableCollection<String> Ignored
        {
            get { return Config.Instance.FilterInfo.IgnoredMobs; }
            set { SetProperty(ref Config.Instance.FilterInfo.IgnoredMobs, value); }
        }

        public ICommand AddIgnoredUnitCommand { get; set; }

        public ICommand DeleteIgnoredUnitCommand { get; set; }

        public ICommand ClearIgnoredUnitsCommand { get; set; }
    }
}
