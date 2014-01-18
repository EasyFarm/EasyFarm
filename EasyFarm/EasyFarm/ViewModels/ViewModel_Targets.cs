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
                RaisePropertyChanged("TargetsName");
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
                RaisePropertyChanged("KillAggro");
            }
        }

        public bool KillUnclaimed
        {
            get { return Engine.Config.BattleUnclaimed; }
            set
            {
                Engine.Config.BattleUnclaimed = value;
                RaisePropertyChanged("KillUnclaimed");
            }
        }

        public bool KillPartyClaimed
        {
            get { return Engine.Config.BattlePartyClaimed; }
            set
            {
                Engine.Config.BattlePartyClaimed = value;
                RaisePropertyChanged("KillPartyClaimed");
            }
        }

        public ICommand AddTargetUnitCommand { get; set; }

        public ICommand DeleteTargetUnitCommand { get; set; }

        public ICommand ClearTargetUnitsCommand { get; set; }
    }
}