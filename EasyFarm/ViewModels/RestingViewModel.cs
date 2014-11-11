
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
using ZeroLimits.FarmingTool;

namespace EasyFarm.ViewModels
{
    public class RestingViewModel : ViewModelBase
    {
        public RestingViewModel() { }

        public int LowHP
        {
            get { return Config.Instance.RestingInfo.Health.Low; }
            set
            {
                SetProperty(ref Config.Instance.RestingInfo.Health.Low, value);
                InformUser("Low hp set to " + this.LowHP);
            }
        }

        public int HighHP
        {
            get { return Config.Instance.RestingInfo.Health.High; }
            set
            {
                SetProperty(ref Config.Instance.RestingInfo.Health.High, value);
                InformUser("High hp set to " + this.HighHP);
            }
        }

        public int LowMP
        {
            get { return Config.Instance.RestingInfo.Magic.Low; }
            set
            {
                SetProperty(ref Config.Instance.RestingInfo.Magic.Low, value);
                InformUser("Low mp set to " + this.LowMP);
            }
        }

        public int HighMP
        {
            get { return Config.Instance.RestingInfo.Magic.High; }
            set
            {
                SetProperty(ref Config.Instance.RestingInfo.Magic.High, value);
                InformUser("High mp set to " + this.HighMP);
            }
        }

        public bool HPEnabled
        {
            get { return Config.Instance.RestingInfo.Health.Enabled; }
            set { SetProperty(ref Config.Instance.RestingInfo.Health.Enabled, value); }
        }

        public bool MPEnabled
        {
            get { return Config.Instance.RestingInfo.Magic.Enabled; }
            set { SetProperty(ref Config.Instance.RestingInfo.Magic.Enabled, value); }
        }
    }
}