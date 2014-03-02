
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
*////////////////////////////////////////////////////////////////////

﻿using EasyFarm.MVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EasyFarm
{
    partial class ViewModel
    {
        public String IgnoredName
        {
            get { return Engine.Config.IgnoredName; }
            set
            {
                Engine.Config.IgnoredName = value;
                RaisePropertyChanged("IgnoredName");
            }
        }

        public ObservableCollection<String> Ignored
        {
            get { return Engine.Config.IgnoredList; }
            set
            {
                Engine.Config.IgnoredList = value;
            }
        }

        public ICommand AddIgnoredUnitCommand { get; set; }

        public ICommand DeleteIgnoredUnitCommand { get; set; }

        public ICommand ClearIgnoredUnitsCommand { get; set; }
    }
}
