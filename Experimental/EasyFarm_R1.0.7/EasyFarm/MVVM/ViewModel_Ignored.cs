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
                OnPropertyChanged("IgnoredName");
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

        public ICommand AddIgnoredUnitCommand
        {
            get
            {
                return new RelayCommand(
                    Action      => AddUnit(Ignored, IgnoredName),
                    Condition   => IsAddable(Ignored, IgnoredName));
            }
        }

        public ICommand DeleteIgnoredUnitCommand
        {
            get
            {
                return new RelayCommand(
                    Action      => DeleteUnit(Ignored, IgnoredName),
                    Condition   => !IsAddable(Ignored, IgnoredName));
            }
        }

        public ICommand ClearIgnoredUnitsCommand
        {
            get
            {
                return new RelayCommand(
                    Action      => ClearUnits(Ignored),
                    Condition   => { return true; }
                );
            }
        }
    }
}
