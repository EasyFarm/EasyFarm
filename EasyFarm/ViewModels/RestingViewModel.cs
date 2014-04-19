
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.ViewModels
{
    public class RestingViewModel : ViewModelBase
    {
        public RestingViewModel(ref GameEngine Engine) : base(ref Engine) { }

        public int LowHP
        {
            get { return GameEngine.Config.RestingInfo.LowHP; }
            set
            {
                GameEngine.Config.RestingInfo.LowHP = value;
                this.OnPropertyChanged(() => this.LowHP);
            }
        }

        public int HighHP
        {
            get { return GameEngine.Config.RestingInfo.HighHP; }
            set
            {
                GameEngine.Config.RestingInfo.HighHP = value;
                this.OnPropertyChanged(() => this.HighHP);
            }
        }

        public int LowMP
        {
            get { return GameEngine.Config.RestingInfo.LowMP; }
            set
            {
                GameEngine.Config.RestingInfo.LowMP = value;
                this.OnPropertyChanged(() => this.LowHP);
            }
        }

        public int HighMP
        {
            get { return GameEngine.Config.RestingInfo.HighMP; }
            set
            {
                GameEngine.Config.RestingInfo.HighMP = value;
                this.OnPropertyChanged(() => this.HighMP);
            }
        }

        public bool HPEnabled
        {
            get { return GameEngine.Config.RestingInfo.IsRestingHPEnabled; }
            set
            {
                GameEngine.Config.RestingInfo.IsRestingHPEnabled = value;
                this.OnPropertyChanged(() => this.HPEnabled);
            }
        }

        public bool MPEnabled
        {
            get { return GameEngine.Config.RestingInfo.IsRestingMPEnabled; }
            set
            {
                GameEngine.Config.RestingInfo.IsRestingMPEnabled = value;
                this.OnPropertyChanged(() => this.MPEnabled);
            }
        }
    }
}