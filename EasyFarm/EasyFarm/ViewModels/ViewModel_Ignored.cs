using EasyFarm.MVVM;
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
