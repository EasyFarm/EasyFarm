
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
using ZeroLimits.XITool;
using ZeroLimits.XITool.Classes;


namespace EasyFarm.ViewModels
{
    [ViewModelAttribute("Settings")]
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            RestoreDefaultsCommand = new DelegateCommand(RestoreDefaults);
        }

        public ICommand RestoreDefaultsCommand { get; set; }
        
        private void RestoreDefaults()
        {
            DetectionDistance = Constants.DETECTION_DISTANCE;
            HeightThreshold = Constants.HEIGHT_THRESHOLD;
            MeleeDistance = Constants.MELEE_DISTANCE;
            WanderDistance = Constants.DETECTION_DISTANCE;
            InformUser("Defaults have been restored.");
        }

        public double DetectionDistance
        {
            get { return Config.Instance.MiscSettings.DetectionDistance; }
            set 
            { 
                SetProperty<double>(ref Config.Instance.MiscSettings.DetectionDistance, (int)value);
                InformUser("Detection Distance Set: {0}.", (int)value);
            }
        }

        public double HeightThreshold
        {
            get { return Config.Instance.MiscSettings.HeightThreshold; }
            set
            {
                SetProperty<double>(ref Config.Instance.MiscSettings.HeightThreshold, value);
                InformUser("Height Threshold Set: {0}.", value);
            }
        }

        public double MeleeDistance
        {
            get { return Config.Instance.MiscSettings.MeleeDistance; }
            set
            {
                SetProperty<double>(ref Config.Instance.MiscSettings.MeleeDistance, value);
                InformUser("Min Melee Distance Set: {0}.", value);
            }
        }

        public double RangedAttackDelay
        {
            get { return Config.Instance.MiscSettings.RangedAttackDelay; }
            set
            {
                SetProperty<double>(ref Config.Instance.MiscSettings.RangedAttackDelay, value);
                InformUser("Ranged Attack Distance Set: {0}.", value);
            }
        }

        public double WanderDistance
        {
            get { return Config.Instance.MiscSettings.WanderDistance; }
            set
            {
                SetProperty<double>(ref Config.Instance.MiscSettings.WanderDistance, (int)value);
                InformUser("Wander Distance Set: {0}.", (int)value);
            }
        }
    }
}
