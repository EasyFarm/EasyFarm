
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

using EasyFarm.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EasyFarm.ViewModels
{
    public class BattlesViewModel : ViewModelBase
    {
        public BattlesViewModel(ref GameEngine Engine) : base(ref Engine) { }

        public Ability BattleAction { get; set; }

        public ObservableCollection<Ability> StartList
        {
            get { return GameEngine.Config.ActionInfo.StartList; }
            set
            {
                GameEngine.Config.ActionInfo.StartList = value;
                this.RaisePropertyChanged("StartList");
            }
        }
        
        public ObservableCollection<Ability> BattleList
        {
            get { return GameEngine.Config.ActionInfo.BattleList; }
            set
            {
                GameEngine.Config.ActionInfo.BattleList = value;
                this.RaisePropertyChanged("BattleList");
            }
        }
        
        public ObservableCollection<Ability> EndList
        {
            get { return GameEngine.Config.ActionInfo.EndList; }
            set
            {
                GameEngine.Config.ActionInfo.EndList = value;
                this.RaisePropertyChanged("EndSelected");
            }

        }

        public bool BattleSelected 
        {
            get { return GameEngine.Config.ActionInfo.BattleListSelected; }
            set
            {
                GameEngine.Config.ActionInfo.BattleListSelected = value;
                this.RaisePropertyChanged("BattleSelected");
            }
        }

        public bool StartSelected
        {
            get { return GameEngine.Config.ActionInfo.StartListSelected; }
            set
            {
                GameEngine.Config.ActionInfo.StartListSelected= value;
                this.RaisePropertyChanged("StartSelected");
            }
        }

        public bool EndSelected
        {
            get { return GameEngine.Config.ActionInfo.EndListSelected; }
            set { GameEngine.Config.ActionInfo.EndListSelected = value;
            this.RaisePropertyChanged("EndSelected");
            }
        }

        public String ActionName
        {
            get { return GameEngine.Config.ActionInfo.BattleActionName; }
            set 
            {
                GameEngine.Config.ActionInfo.BattleActionName = value;
                this.RaisePropertyChanged("ActionName");
                BattleAction = GameEngine.AbilityService.CreateAbility(ActionName);
            }            
        }

        public ICommand AddActionCommand { get; set; }

        public ICommand DeleteActionCommand { get; set; }

        public ICommand ClearActionsCommand { get; set; }

        private ObservableCollection<Ability> SelectedList
        {
            get 
            {
                if (StartSelected)
                    return StartList;
                else if (BattleSelected)
                    return BattleList;
                else if (EndSelected)
                    return EndList;
                else
                    return new ObservableCollection<Ability>();
            }
        }

        private void AddAction(object obj)
        {
            SelectedList.Add(BattleAction);
        }

        private void DeleteAction(object obj)
        {
            SelectedList.Remove(obj as Ability);
        }

        private void ClearActions()
        {
            SelectedList.Clear();
        }

        private bool IsBattleActionAddable(object obj)
        {
            if (BattleAction != null  && BattleAction.IsValidName && !SelectedList.Contains(BattleAction))
                return true;
            else
                return false;
        }

        private bool IsBattleActionRemovable(object obj) 
        { 
            return !IsBattleActionAddable(obj); 
        }
    }
}
