using EasyFarm.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EasyFarm
{
    partial class ViewModel
    {
        /// <summary>
        /// Gets target's name; textbox's text field.
        /// </summary>
        public String TargetsName
        {
            get { return Engine.Config.TargetsName; }
            set
            {
                Engine.Config.TargetsName = value;
                OnPropertyChanged("TargetsName");
            }
        }

        /// <summary>
        /// Gets the list of targets; combo boxes items.
        /// </summary>
        public ObservableCollection<String> Targets
        {
            get { return Engine.Config.TargetsList; }
            set
            {
                Engine.Config.TargetsList = value;
            }
        }

        /// <summary>
        /// Gets the bool value that determines whether we should kill aggro; 
        /// Check box value.
        /// </summary>
        public bool KillAggro
        {
            get { return Engine.Config.BattleAggro; }
            set
            {
                Engine.Config.BattleAggro = value;
                OnPropertyChanged("KillAggro");
            }
        }

        public bool KillUnclaimed
        {
            get { return Engine.Config.BattleUnclaimed; }
            set
            {
                Engine.Config.BattleUnclaimed = value;
                OnPropertyChanged("KillUnclaimed");
            }
        }

        public bool KillPartyClaimed
        {
            get { return Engine.Config.BattlePartyClaimed; }
            set
            {
                Engine.Config.BattlePartyClaimed = value;
                OnPropertyChanged("KillPartyClaimed");
            }
        }

        public ICommand AddTargetUnitCommand
        {
            get
            {
                return new RelayCommand(
                    Action      => AddUnit(Targets, TargetsName),
                    Condition   => IsAddable(Targets, TargetsName));
            }
        }

        public ICommand DeleteTargetUnitCommand
        {
            get
            {
                return new RelayCommand(
                    Action      => DeleteUnit(Targets,TargetsName),
                    Condition   => !IsAddable(Targets, TargetsName));
            }
        }

        public ICommand ClearTargetUnitsCommand
        {
            get
            {
                return new RelayCommand(
                    Action      => ClearUnits(Targets),
                    Condition   => { return true; }
                );
            }
        }
    }
}