using EasyFarm.MVVM;
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
            OnPropertyChanged("HighHP");
            StatusBarText = "UP HP : " + value;
            }
        }

        public int LowMP {
            get { return Engine.Config.LowMP; }
            set { Engine.Config.LowMP = value;
            OnPropertyChanged("LowMP");
            StatusBarText = "Down MP : " + value;
            }
        }

        public int HighMP {
            get { return Engine.Config.HighMP; }
            set { Engine.Config.HighMP = value;
            OnPropertyChanged("HighMP");
            StatusBarText = "UP MP : " + value;
            }
        }

        public bool IsRestingHPEnabled 
        {
            get { return Engine.Config.IsRestingHPEnabled; }
            set { Engine.Config.IsRestingHPEnabled = value;
            OnPropertyChanged("IsRestingHPEnabled");
            }
        }

        public bool IsRestingMPEnabled
        {
            get { return Engine.Config.IsRestingMPEnabled; }
            set { Engine.Config.IsRestingMPEnabled = value;
            OnPropertyChanged("IsRestingMPEnabled");
            }
        }
    }
}