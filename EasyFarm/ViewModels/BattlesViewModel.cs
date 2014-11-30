
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

using EasyFarm.GameData;
using EasyFarm.UserSettings;
using EasyFarm.Views;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZeroLimits.FarmingTool;
using ZeroLimits.XITool;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.ViewModels
{
    [ViewModelAttribute("Battles")]
    public class BattlesViewModel : ViewModelBase
    {
        AbilityService AbilityService = new AbilityService();
        
        public BattlesViewModel()
        {
            AddActionCommand = new DelegateCommand<Object>(AddAction);
            DeleteActionCommand = new DelegateCommand<Object>(DeleteAction);
            ClearActionsCommand = new DelegateCommand(ClearActions);
        }

        private Ability battleAction;

        public Ability BattleAction
        {
            get { return battleAction; }
            set { SetProperty(ref this.battleAction, value); }
        }

        /// <summary>
        /// The list of moves to use before battle.
        /// </summary>
        public ObservableCollection<Ability> StartList
        {
            get { return Config.Instance.ActionInfo.StartList; }
            set { SetProperty(ref Config.Instance.ActionInfo.StartList, value); }
        }

        /// <summary>
        /// The list of moves to use during battle.
        /// </summary>
        public ObservableCollection<Ability> BattleList
        {
            get { return Config.Instance.ActionInfo.BattleList; }
            set { SetProperty(ref Config.Instance.ActionInfo.BattleList, value); }
        }

        /// <summary>
        /// The list of moves to use after battle.
        /// </summary>
        public ObservableCollection<Ability> EndList
        {
            get { return Config.Instance.ActionInfo.EndList; }
            set { SetProperty(ref Config.Instance.ActionInfo.EndList, value); }

        }

        public ObservableCollection<Ability> PullList
        {
            get { return Config.Instance.ActionInfo.PullList; }
            set { SetProperty(ref Config.Instance.ActionInfo.PullList, value); }
        }

        /// <summary>
        /// When selected displays the battle moves list.
        /// </summary>
        public bool BattleSelected
        {
            get { return Config.Instance.ActionInfo.BattleListSelected; }
            set { SetProperty(ref Config.Instance.ActionInfo.BattleListSelected, value); }
        }

        /// <summary>
        /// When selected displays the starting moves list.
        /// </summary>
        public bool StartSelected
        {
            get { return Config.Instance.ActionInfo.StartListSelected; }
            set { SetProperty(ref Config.Instance.ActionInfo.StartListSelected, value); }
        }

        /// <summary>
        /// When selected displays the ending moves list.
        /// </summary>
        public bool EndSelected
        {
            get { return Config.Instance.ActionInfo.EndListSelected; }
            set { SetProperty(ref Config.Instance.ActionInfo.EndListSelected, value); }
        }

        /// <summary>
        /// When selected displays the ending moves list.
        /// </summary>
        public bool PullSelected
        {
            get { return Config.Instance.ActionInfo.PullListSelected; }
            set { SetProperty(ref Config.Instance.ActionInfo.PullListSelected, value); }
        }

        /// <summary>
        /// Private backing for the moves name.
        /// </summary>
        private String actionName;

        /// <summary>
        /// The string to be turned into a battle move.
        /// </summary>
        public String ActionName
        {
            get { return actionName; }
            set { SetProperty(ref this.actionName, value); }
        }

        /// <summary>
        /// Action to add an new move to the currently selected list.
        /// </summary>
        public ICommand AddActionCommand { get; set; }

        /// <summary>
        /// Action to delete an existing move from the currently selected list.
        /// </summary>
        public ICommand DeleteActionCommand { get; set; }

        /// <summary>
        /// Action to clear all moves from the currently selected list.
        /// </summary>
        public ICommand ClearActionsCommand { get; set; }

        /// <summary>
        /// Returns the currently selected list.
        /// </summary>
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
                else if (PullSelected)
                    return PullList;
                else
                    return new ObservableCollection<Ability>();
            }
        }

        /// <summary>
        /// Add an move to the currently selected list.
        /// </summary>
        /// <param name="obj"></param>
        private void AddAction(object obj)
        {
            String name = obj as String;
            if (AbilityService.GetAbilitiesWithName(name).Count > 1)
                SelectedList.Add(new AbilitySelectionBox(name).SelectedAbility);
            else if (AbilityService.Exists(name))
                SelectedList.Add(AbilityService.CreateAbility(name));
        }

        /// <summary>
        /// Remove an move from the currently selected list.
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteAction(object obj)
        {
            SelectedList.Remove(obj as Ability);
        }

        /// <summary>
        /// Clear the currently selected list.
        /// </summary>
        private void ClearActions()
        {
            SelectedList.Clear();
        }

        /// <summary>
        /// Returns whether we can add an move.
        /// </summary>
        /// <param name="obj">Move's name</param>
        /// <returns></returns>
        private bool IsBattleActionAddable(object obj)
        {
            if (BattleAction != null && BattleAction.IsValidName && !SelectedList.Contains(BattleAction))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns whether we can delete an move.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool IsBattleActionRemovable(object obj)
        {
            return !IsBattleActionAddable(obj);
        }
    }
}
