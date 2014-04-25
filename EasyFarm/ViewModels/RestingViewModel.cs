
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
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.ViewModels
{
    public class RestingViewModel : ViewModelBase
    {
        public RestingViewModel(ref GameEngine Engine, IEventAggregator eventAggregator) : 
            base(ref Engine, eventAggregator) { }

        public int LowHP
        {
            get { return GameEngine.Config.RestingInfo.Health.Low; }
            set
            {
                GameEngine.Config.RestingInfo.Health.Low = value;
                this.OnPropertyChanged(() => this.LowHP);
                InformUser("Low hp set to " + this.LowHP);
            }
        }

        public int HighHP
        {
            get { return GameEngine.Config.RestingInfo.Health.High; }
            set
            {
                GameEngine.Config.RestingInfo.Health.High = value;
                this.OnPropertyChanged(() => this.HighHP);
                InformUser("High hp set to " + this.HighHP);
            }
        }

        public int LowMP
        {
            get { return GameEngine.Config.RestingInfo.Magic.Low; }
            set
            {
                GameEngine.Config.RestingInfo.Magic.Low = value;
                this.OnPropertyChanged(() => this.LowHP);
                InformUser("Low mp set to " + this.LowMP);
            }
        }

        public int HighMP
        {
            get { return GameEngine.Config.RestingInfo.Magic.High; }
            set
            {
                GameEngine.Config.RestingInfo.Magic.High = value;
                this.OnPropertyChanged(() => this.HighMP);
                InformUser("High mp set to " + this.HighMP);
            }
        }

        public bool HPEnabled
        {
            get { return GameEngine.Config.RestingInfo.Health.Enabled; }
            set
            {
                GameEngine.Config.RestingInfo.Health.Enabled = value;
                this.OnPropertyChanged(() => this.HPEnabled);
            }
        }

        public bool MPEnabled
        {
            get { return GameEngine.Config.RestingInfo.Magic.Enabled; }
            set
            {
                GameEngine.Config.RestingInfo.Magic.Enabled = value;
                this.OnPropertyChanged(() => this.MPEnabled);
            }
        }
    }
}