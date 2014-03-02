
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EasyFarm
{
    partial class ViewModel
    {
        public int LowHP {
            get { return Engine.Config.LowHP; }
            set { Engine.Config.LowHP = value;
            StatusBarText = "Down HP : " + value;
            }
        }
        
        public int HighHP {
            get { return Engine.Config.HighHP; }
            set { Engine.Config.HighHP = value;
            RaisePropertyChanged("HighHP");
            StatusBarText = "UP HP : " + value;
            }
        }

        public int LowMP {
            get { return Engine.Config.LowMP; }
            set { Engine.Config.LowMP = value;
            RaisePropertyChanged("LowMP");
            StatusBarText = "Down MP : " + value;
            }
        }

        public int HighMP {
            get { return Engine.Config.HighMP; }
            set { Engine.Config.HighMP = value;
            RaisePropertyChanged("HighMP");
            StatusBarText = "UP MP : " + value;
            }
        }

        public bool IsRestingHPEnabled 
        {
            get { return Engine.Config.IsRestingHPEnabled; }
            set { Engine.Config.IsRestingHPEnabled = value;
            RaisePropertyChanged("IsRestingHPEnabled");
            }
        }

        public bool IsRestingMPEnabled
        {
            get { return Engine.Config.IsRestingMPEnabled; }
            set { Engine.Config.IsRestingMPEnabled = value;
            RaisePropertyChanged("IsRestingMPEnabled");
            }
        }
    }
}