
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

using EasyFarm.Classes;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;

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
            GlobalCooldown = Constants.GLOBAL_SPELL_COOLDOWN;
            AppInformer.InformUser("Defaults have been restored.");
        }

        public double DetectionDistance
        {
            get { return Config.Instance.DetectionDistance; }
            set 
            { 
                SetProperty<double>(ref Config.Instance.DetectionDistance, (int)value);
                AppInformer.InformUser("Detection Distance Set: {0}.", (int)value);
            }
        }

        public double HeightThreshold
        {
            get { return Config.Instance.HeightThreshold; }
            set
            {
                SetProperty<double>(ref Config.Instance.HeightThreshold, value);
                AppInformer.InformUser("Height Threshold Set: {0:F1}.", value);
            }
        }

        public double MeleeDistance
        {
            get { return Config.Instance.MeleeDistance; }
            set
            {
                SetProperty<double>(ref Config.Instance.MeleeDistance, value);
                AppInformer.InformUser("Melee Distance Set: {0:F1}.", value);
            }
        }        

        public double WanderDistance
        {
            get { return Config.Instance.WanderDistance; }
            set
            {
                SetProperty<double>(ref Config.Instance.WanderDistance, (int)value);
                AppInformer.InformUser("Wander Distance Set: {0}.", (int)value);
            }
        }

        public int GlobalCooldown
        {
            get { return Config.Instance.GlobalCooldown; }
            set
            {
                SetProperty(ref Config.Instance.GlobalCooldown, (int)value);
                AppInformer.InformUser("Global Cooldown Set: {0}.", (int)value);
            }
        }
    }
}