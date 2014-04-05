
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

﻿using EasyFarm.Classes;
using EasyFarm.MVVM;
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
            get { return Engine.Config.ActionInfo.StartList; }
            set { Engine.Config.ActionInfo.StartList = value; }
        }
        
        public ObservableCollection<Ability> BattleList
        {
            get { return Engine.Config.ActionInfo.BattleList; }
            set { Engine.Config.ActionInfo.BattleList= value; }
        }
        
        public ObservableCollection<Ability> EndList
        {
            get { return Engine.Config.ActionInfo.EndList; }
            set { Engine.Config.ActionInfo.EndList = value; }
        }

        public bool BattleListSelected 
        {
            get { return Engine.Config.ActionInfo.BattleListSelected; }
            set { Engine.Config.ActionInfo.BattleListSelected = value;
            RaisePropertyChanged("BattleListSelected");
            }
        }

        public bool StartListSelected 
        {
            get { return Engine.Config.ActionInfo.StartListSelected; }
            set { Engine.Config.ActionInfo.StartListSelected = value;
            RaisePropertyChanged("StartListSelected");
            }
        }

        public bool EndListSelected 
        {
            get { return Engine.Config.ActionInfo.EndListSelected; }
            set { Engine.Config.ActionInfo.EndListSelected = value;
            RaisePropertyChanged("EndListSelected");
            }
        }

        public String BattleActionName
        {
            get { return Engine.Config.ActionInfo.BattleActionName; }
            set { Engine.Config.ActionInfo.BattleActionName = value;
            RaisePropertyChanged("BattleActionName");
            BattleAction = Engine.AbilityService.CreateAbility(BattleActionName) as Ability;
            }
        }

        public ICommand AddActionCommand { get; set; }

        public ICommand DeleteActionCommand { get; set; }

        public ICommand ClearActionsCommand { get; set; }

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
