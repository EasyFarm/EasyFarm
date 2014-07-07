
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
*/
///////////////////////////////////////////////////////////////////

using EasyFarm.Classes;
using EasyFarm.Classes.Services;
using EasyFarm.Views;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EasyFarm.ViewModels
{
    public class BattlesViewModel : ViewModelBase
    {
        public BattlesViewModel(FarmingTools farmingTools)
            : base(farmingTools) 
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
            get { return farmingTools.UserSettings.ActionInfo.StartList; }
            set { SetProperty(ref this.farmingTools.UserSettings.ActionInfo.StartList, value); }
        }

        /// <summary>
        /// The list of moves to use during battle.
        /// </summary>
        public ObservableCollection<Ability> BattleList
        {
            get { return farmingTools.UserSettings.ActionInfo.BattleList; }
            set { SetProperty(ref this.farmingTools.UserSettings.ActionInfo.BattleList, value); }
        }

        /// <summary>
        /// The list of moves to use after battle.
        /// </summary>
        public ObservableCollection<Ability> EndList
        {
            get { return farmingTools.UserSettings.ActionInfo.EndList; }
            set { SetProperty(ref this.farmingTools.UserSettings.ActionInfo.EndList, value); }

        }

        /// <summary>
        /// When selected displays the battle moves list.
        /// </summary>
        public bool BattleSelected
        {
            get { return farmingTools.UserSettings.ActionInfo.BattleListSelected; }
            set { SetProperty(ref this.farmingTools.UserSettings.ActionInfo.BattleListSelected, value); }
        }

        /// <summary>
        /// When selected displays the starting moves list.
        /// </summary>
        public bool StartSelected
        {
            get { return farmingTools.UserSettings.ActionInfo.StartListSelected; }
            set { SetProperty(ref this.farmingTools.UserSettings.ActionInfo.StartListSelected, value); }
        }

        /// <summary>
        /// When selected displays the ending moves list.
        /// </summary>
        public bool EndSelected
        {
            get { return farmingTools.UserSettings.ActionInfo.EndListSelected; }
            set { SetProperty(ref this.farmingTools.UserSettings.ActionInfo.EndListSelected, value); }
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
            if (farmingTools.AbilityService.GetAbilitiesWithName(name).Count > 1)
                SelectedList.Add(new AbilitySelectionBox(name).SelectedAbility);
            else if (farmingTools.AbilityService.Exists(name))
                SelectedList.Add(farmingTools.AbilityService.CreateAbility(name));
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
