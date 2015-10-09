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
using EasyFarm.Mvvm;

namespace EasyFarm.ViewModels
{
    [ViewModelAttribute("Resting")]
    public class RestingViewModel : ViewModelBase
    {
        public int LowHp
        {
            get { return Config.Instance.LowHealth; }
            set
            {
                SetProperty(ref Config.Instance.LowHealth, value);
                AppInformer.InformUser("Low hp set to " + LowHp);
            }
        }

        public int HighHp
        {
            get { return Config.Instance.HighHealth; }
            set
            {
                SetProperty(ref Config.Instance.HighHealth, value);
                AppInformer.InformUser("High hp set to " + HighHp);
            }
        }

        public int LowMp
        {
            get { return Config.Instance.LowMagic; }
            set
            {
                SetProperty(ref Config.Instance.LowMagic, value);
                AppInformer.InformUser("Low mp set to " + LowMp);
            }
        }

        public int HighMp
        {
            get { return Config.Instance.HighMagic; }
            set
            {
                SetProperty(ref Config.Instance.HighMagic, value);
                AppInformer.InformUser("High mp set to " + HighMp);
            }
        }

        public bool HpEnabled
        {
            get { return Config.Instance.IsHealthEnabled; }
            set { SetProperty(ref Config.Instance.IsHealthEnabled, value); }
        }

        public bool MpEnabled
        {
            get { return Config.Instance.IsMagicEnabled; }
            set { SetProperty(ref Config.Instance.IsMagicEnabled, value); }
        }
    }
}