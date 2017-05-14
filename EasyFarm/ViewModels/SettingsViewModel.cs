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

using System.Windows.Input;
using EasyFarm.Classes;
using EasyFarm.Infrastructure;
using Prism.Commands;

namespace EasyFarm.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            RestoreDefaultsCommand = new DelegateCommand(RestoreDefaults);
            ViewName = "Settings";
        }

        public ICommand RestoreDefaultsCommand { get; set; }

        public bool ShouldEngage
        {
            get { return Config.Instance.IsEngageEnabled; }
            set { SetProperty(ref Config.Instance.IsEngageEnabled, value); }
        }

        public bool ShouldApproach
        {
            get { return Config.Instance.IsApproachEnabled; }
            set { SetProperty(ref Config.Instance.IsApproachEnabled, value); }
        }

        public double DetectionDistance
        {
            get { return Config.Instance.DetectionDistance; }
            set
            {
                SetProperty(ref Config.Instance.DetectionDistance, (int) value);
                AppServices.InformUser("Detection Distance Set: {0}.", (int) value);
            }
        }

        public double HeightThreshold
        {
            get { return Config.Instance.HeightThreshold; }
            set
            {
                SetProperty(ref Config.Instance.HeightThreshold, value);
                AppServices.InformUser("Height Threshold Set: {0:F1}.", value);
            }
        }

        public double MeleeDistance
        {
            get { return Config.Instance.MeleeDistance; }
            set
            {
                SetProperty(ref Config.Instance.MeleeDistance, value);
                AppServices.InformUser("Melee Distance Set: {0:F1}.", value);
            }
        }

        public double WanderDistance
        {
            get { return Config.Instance.WanderDistance; }
            set
            {
                SetProperty(ref Config.Instance.WanderDistance, (int) value);
                AppServices.InformUser("Wander Distance Set: {0}.", (int) value);
            }
        }

        public int GlobalCooldown
        {
            get { return Config.Instance.GlobalCooldown; }
            set
            {
                SetProperty(ref Config.Instance.GlobalCooldown, value);
                AppServices.InformUser("Global Cooldown Set: {0}.", value);
            }
        }

        public bool AvoidObjects
        {
            get { return Config.Instance.IsObjectAvoidanceEnabled; }
            set
            {
                SetProperty(ref Config.Instance.IsObjectAvoidanceEnabled, value);
            }
        }

        public bool EnableTabTargeting
        {
            get { return Config.Instance.EnableTabTargeting; }
            set
            {
                SetProperty(ref Config.Instance.EnableTabTargeting, value);
            }
        }

        public bool HomePointOnDeath
        {
            get { return Config.Instance.HomePointOnDeath; }
            set
            {
                SetProperty(ref Config.Instance.HomePointOnDeath, value);
            }
        }

        public int TrustPartySize
        {
            get { return Config.Instance.TrustPartySize; }
            set
            {
                SetProperty(ref Config.Instance.TrustPartySize, value);
            }
        }

        private void RestoreDefaults()
        {
            DetectionDistance = Constants.DetectionDistance;
            HeightThreshold = Constants.HeightThreshold;
            MeleeDistance = Constants.MeleeDistance;
            WanderDistance = Constants.DetectionDistance;
            GlobalCooldown = Constants.GlobalSpellCooldown;
            EnableTabTargeting = false;
            AvoidObjects = false;
            ShouldApproach = true;
            ShouldEngage = true;
            HomePointOnDeath = false;
            TrustPartySize = Constants.TrustPartySize;
            AppServices.InformUser("Defaults have been restored.");
        }        
    }
}