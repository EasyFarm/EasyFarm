
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
using FFACETools;

namespace EasyFarm.ViewModels
{
    [ViewModelAttribute("Resting")]
    public class RestingViewModel : ViewModelBase
    {
        public int LowHP
        {
            get { return Config.Instance.LowHealth; }
            set
            {
                SetProperty(ref Config.Instance.LowHealth, value);
                InformUser("Low hp set to " + LowHP);
            }
        }

        public int HighHP
        {
            get { return Config.Instance.HighHealth; }
            set
            {
                SetProperty(ref Config.Instance.HighHealth, value);
                InformUser("High hp set to " + HighHP);
            }
        }

        public int LowMP
        {
            get { return Config.Instance.LowMagic; }
            set
            {
                SetProperty(ref Config.Instance.LowMagic, value);
                InformUser("Low mp set to " + LowMP);
            }
        }

        public int HighMP
        {
            get { return Config.Instance.HighMagic; }
            set
            {
                SetProperty(ref Config.Instance.HighMagic, value);
                InformUser("High mp set to " + HighMP);
            }
        }

        public bool HPEnabled
        {
            get { return Config.Instance.IsHealthEnabled; }
            set { SetProperty(ref Config.Instance.IsHealthEnabled, value); }
        }

        public bool MPEnabled
        {
            get { return Config.Instance.IsMagicEnabled; }
            set { SetProperty(ref Config.Instance.IsMagicEnabled, value); }
        }
    }
}

namespace EasyFarm.UserSettings
{
    // Health data. 
    public partial class Config
    {
        public bool IsHealthEnabled = false;
        public int HighHealth = 100;
        public int LowHealth = 50;

        public bool ShouldRestForHealth(int health, Status status)
        {
            // Rest while low and while not high
            return (IsHealthEnabled && (IsHealthLow(health) || !IsHealthHigh(health) && status == Status.Healing));
        }

        public bool IsHealthLow(int health)
        {
            return health <= LowHealth;
        }

        public bool IsHealthHigh(int health)
        {
            return health >= HighHealth;
        }
    }

    // Magic Data. 
    public partial class Config
    {
        public bool IsMagicEnabled = false;
        public int HighMagic = 100;
        public int LowMagic = 50;

        public bool ShouldRestForMagic(int magic, Status status)
        {
            return (IsMagicEnabled && (IsMagicLow(magic) || !IsMagicHigh(magic) && status == Status.Healing));
        }

        public bool IsMagicLow(int magic)
        {
            return magic <= LowMagic;
        }

        public bool IsMagicHigh(int magic)
        {
            return magic >= HighMagic;
        }
    }
}