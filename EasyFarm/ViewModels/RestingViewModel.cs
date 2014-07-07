
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
using EasyFarm.Classes.Services;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.ViewModels
{
    public class RestingViewModel : ViewModelBase
    {
        public RestingViewModel(FarmingTools farmingTools) : base(farmingTools) { }

        public int LowHP
        {
            get { return farmingTools.UserSettings.RestingInfo.Health.Low; }
            set
            {
                SetProperty(ref farmingTools.UserSettings.RestingInfo.Health.Low, value);
                App.InformUser("Low hp set to " + this.LowHP);
            }
        }

        public int HighHP
        {
            get { return farmingTools.UserSettings.RestingInfo.Health.High; }
            set
            {
                SetProperty(ref farmingTools.UserSettings.RestingInfo.Health.High, value);
                App.InformUser("High hp set to " + this.HighHP);
            }
        }

        public int LowMP
        {
            get { return farmingTools.UserSettings.RestingInfo.Magic.Low; }
            set
            {
                SetProperty(ref farmingTools.UserSettings.RestingInfo.Magic.Low, value);
                App.InformUser("Low mp set to " + this.LowMP);
            }
        }

        public int HighMP
        {
            get { return farmingTools.UserSettings.RestingInfo.Magic.High; }
            set
            {
                SetProperty(ref farmingTools.UserSettings.RestingInfo.Magic.High, value);
                App.InformUser("High mp set to " + this.HighMP);
            }
        }

        public bool HPEnabled
        {
            get { return farmingTools.UserSettings.RestingInfo.Health.Enabled; }
            set { SetProperty(ref farmingTools.UserSettings.RestingInfo.Health.Enabled, value); }
        }

        public bool MPEnabled
        {
            get { return farmingTools.UserSettings.RestingInfo.Magic.Enabled; }
            set { SetProperty(ref farmingTools.UserSettings.RestingInfo.Magic.Enabled, value); }
        }
    }
}