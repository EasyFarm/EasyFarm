
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


namespace EasyFarm.ViewModels
{
    public class UnitFilteringViewModel : ViewModelBase
    {
        public UnitFilteringViewModel(FarmingTools farmingTools) : base(farmingTools) 
        {
            RestoreDefaultsCommand = new DelegateCommand(RestoreDefaults);
        }

        public ICommand RestoreDefaultsCommand { get; set; }
        
        private void RestoreDefaults()
        {
            DetectionDistance = Constants.DETECTION_DISTANCE;
            HeightThreshold = Constants.HEIGHT_THRESHOLD;
            MeleeDistance = Constants.MELEE_DISTANCE;
            App.InformUser("Defaults have been restored.");
        }

        public double DetectionDistance
        {
            get { return farmingTools.UserSettings.MiscSettings.DetectionDistance; }
            set 
            { 
                SetProperty<double>(ref farmingTools.UserSettings.MiscSettings.DetectionDistance, value);
                App.InformUser("Detection Distance Set: {0}.", value);
            }
        }

        public double HeightThreshold
        {
            get { return farmingTools.UserSettings.MiscSettings.HeightThreshold; }
            set
            {
                SetProperty<double>(ref farmingTools.UserSettings.MiscSettings.HeightThreshold, value);
                App.InformUser("Height Threshold Set: {0}.", value);
            }
        }

        public double MeleeDistance
        {
            get { return farmingTools.UserSettings.MiscSettings.MeleeDistance; }
            set
            {
                SetProperty<double>(ref farmingTools.UserSettings.MiscSettings.MeleeDistance, value);
                App.InformUser("Min Melee Distance Set: {0}.", value);
            }
        }
    }
}
