
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

using EasyFarm.Models;
using EasyFarm.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZeroLimits.FarmingTool;
using ZeroLimits.XITools;


namespace EasyFarm.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public static SettingsModel SettingsModel { get; set; }

        public SettingsViewModel() 
        {
            RestoreDefaultsCommand = new DelegateCommand(RestoreDefaults);
        }

        public ICommand RestoreDefaultsCommand { get; set; }
        
        private void RestoreDefaults()
        {
            MainViewModel.MainModel.InformUser("Defaults have been restored.");
        }

        public double DetectionDistance
        {
            get { return SettingsModel.DetectionDistance; }
            set 
            {
                SetProperty(ref SettingsModel.DetectionDistance, value);
                MainViewModel.MainModel.InformUser("Detection Distance Set: {0}.", value);
            }
        }

        public double HeightThreshold
        {
            get { return SettingsModel.HeightThreshold; }
            set
            {
                SetProperty(ref SettingsModel.HeightThreshold, value);
                MainViewModel.MainModel.InformUser("Height Threshold Set: {0}.", value);
            }
        }

        public double MeleeDistance
        {
            get { return SettingsModel.MeleeDistance; }
            set
            {
                SetProperty(ref SettingsModel.MeleeDistance, value);
                MainViewModel.MainModel.InformUser("Min Melee Distance Set: {0}.", value);
            }
        }
    }
}
