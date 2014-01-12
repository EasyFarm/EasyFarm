using EasyFarm.MVVM;
using EasyFarm.PlayerTools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EasyFarm
{
    partial class ViewModel
    {
        private Ability BattleAction;

        public ObservableCollection<Ability> StartList
        {
            get { return Engine.Config.StartList; }
            set { Engine.Config.StartList = value; }
        }
        
        public ObservableCollection<Ability> BattleList
        {
            get { return Engine.Config.BattleList; }
            set { Engine.Config.BattleList= value; }
        }
        
        public ObservableCollection<Ability> EndList
        {
            get { return Engine.Config.EndList; }
            set { Engine.Config.EndList = value; }
        }

        public bool BattleListSelected 
        {
            get { return Engine.Config.BattleListSelected; }
            set { Engine.Config.BattleListSelected = value;
            OnPropertyChanged("BattleListSelected");
            }
        }

        public bool StartListSelected 
        {
            get { return Engine.Config.StartListSelected; }
            set { Engine.Config.StartListSelected = value;
            OnPropertyChanged("StartListSelected");
            }
        }

        public bool EndListSelected 
        {
            get { return Engine.Config.EndListSelected; }
            set { Engine.Config.EndListSelected = value;
            OnPropertyChanged("EndListSelected");
            }
        }

        public String BattleActionName
        {
            get { return Engine.Config.BattleActionName; }
            set { Engine.Config.BattleActionName = value;
            OnPropertyChanged("BattleActionName");
            BattleAction = new Ability(BattleActionName);
            }
        }

        public ICommand AddActionCommand
        {            
            get
            {
                return new RelayCommand(
                    Action => { AddAction(); },
                    Condition => { return IsBattleActionAddable(); }
                );
            }
        }

        public ICommand DeleteActionCommand
        {
            get 
            {
                return new RelayCommand(
                    Action => { DeleteAction(); },
                    Condition => { return !IsBattleActionAddable(); }
                    );
            }
        }

        public ICommand ClearActionsCommand
        {
            get
            {
                return new RelayCommand(
                    Action => { ClearActions(); },
                    Condition => { return true; }
                );
            }
        }

        private ObservableCollection<Ability> SelectedList
        {
            get 
            {
                if (StartListSelected)
                    return StartList;
                else if (BattleListSelected)
                    return BattleList;
                else if (EndListSelected)
                    return EndList;
                else
                    return new ObservableCollection<Ability>();
            }
        }

        private void AddAction()
        {
            SelectedList.Add(BattleAction);
        }

        private void DeleteAction()
        {
            SelectedList.Remove(BattleAction);
        }

        private void ClearActions()
        {
            SelectedList.Clear();
        }

        private bool IsBattleActionAddable()
        {
            if (BattleAction != null  && BattleAction.IsValidName && !SelectedList.Contains(BattleAction))
                return true;
            else
                return false;
        }
    }
}
