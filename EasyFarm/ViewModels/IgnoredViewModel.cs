
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EasyFarm.ViewModels
{
    public class IgnoredViewModel : ViewModelBase
    {
        public IgnoredViewModel(ref GameEngine Engine)
            : base(ref Engine)
        {
            AddIgnoredUnitCommand = new RelayCommand(AddIgnoredUnit);
            DeleteIgnoredUnitCommand = new RelayCommand(DeleteIgnoredUnit);
            ClearIgnoredUnitsCommand = new RelayCommand(ClearIgnoredUnits);
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
            get { return GameEngine.Config.FilterInfo.IgnoredName; }
            set
            {
                GameEngine.Config.FilterInfo.IgnoredName = value;
                this.RaisePropertyChanged("Name");
            }
        }

        public ObservableCollection<String> Ignored
        {
            get { return GameEngine.Config.FilterInfo.IgnoredMobs; }
            set
            {
                GameEngine.Config.FilterInfo.IgnoredMobs = value;
                this.RaisePropertyChanged("Ignored");
            }
        }

        public ICommand AddIgnoredUnitCommand { get; set; }

        public ICommand DeleteIgnoredUnitCommand { get; set; }

        public ICommand ClearIgnoredUnitsCommand { get; set; }
    }
}
