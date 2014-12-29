
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

using EasyFarm.UserSettings;
using EasyFarm.Views;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZeroLimits.XITool.Classes;
using System.Linq;
using EasyFarm.Classes;

namespace EasyFarm.ViewModels
{
    [ViewModelAttribute("Battles")]
    public class BattlesViewModel : ViewModelBase
    {
        readonly AbilityService _abilityService = new AbilityService();

        public BattlesViewModel()
        {
            AddActionCommand = new DelegateCommand<Object>(AddAction);
            DeleteActionCommand = new DelegateCommand(DeleteAction);
            ClearActionsCommand = new DelegateCommand(ClearActions);
            SetCommand = new DelegateCommand(SetAbility);
        }

        public int SelectedIndex { get; set; }

        private void SetAbility()
        {
            if (SelectedIndex < 0) return;

            var selected = this.SelectedList.ElementAt(SelectedIndex);

            if (selected.SetAbility())
                ViewModelBase.InformUser(selected.NameX + " set successfully. ");
        }

        /// <summary>
        /// The list of moves to use before battle.
        /// </summary>
        public ObservableCollection<BattleAbility> StartList
        {
            get { return Config.Instance.ActionInfo.StartList; }
            set { SetProperty(ref Config.Instance.ActionInfo.StartList, value); }
        }

        /// <summary>
        /// The list of moves to use during battle.
        /// </summary>
        public ObservableCollection<BattleAbility> BattleList
        {
            get { return Config.Instance.ActionInfo.BattleList; }
            set { SetProperty(ref Config.Instance.ActionInfo.BattleList, value); }
        }

        /// <summary>
        /// The list of moves to use after battle.
        /// </summary>
        public ObservableCollection<BattleAbility> EndList
        {
            get { return Config.Instance.ActionInfo.EndList; }
            set { SetProperty(ref Config.Instance.ActionInfo.EndList, value); }

        }

        public ObservableCollection<BattleAbility> PullList
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
            set
            {
                SetProperty(ref Config.Instance.ActionInfo.BattleListSelected, value);
                // Ensures our lists have atleast 
                // one ability item in them. 
                KeepOne();
            }
        }

        /// <summary>
        /// When selected displays the starting moves list.
        /// </summary>
        public bool StartSelected
        {
            get { return Config.Instance.ActionInfo.StartListSelected; }
            set
            {
                SetProperty(ref Config.Instance.ActionInfo.StartListSelected, value);
                KeepOne();
            }
        }

        /// <summary>
        /// When selected displays the ending moves list.
        /// </summary>
        public bool EndSelected
        {
            get { return Config.Instance.ActionInfo.EndListSelected; }
            set
            {
                SetProperty(ref Config.Instance.ActionInfo.EndListSelected, value);
                KeepOne();
            }
        }

        /// <summary>
        /// When selected displays the ending moves list.
        /// </summary>
        public bool PullSelected
        {
            get { return Config.Instance.ActionInfo.PullListSelected; }
            set
            {
                SetProperty(ref Config.Instance.ActionInfo.PullListSelected, value);
                KeepOne();
            }
        }

        /// <summary>
        /// Private backing for the moves name.
        /// </summary>
        private String _actionName;

        /// <summary>
        /// The string to be turned into a battle move.
        /// </summary>
        public String ActionName
        {
            get { return _actionName; }
            set { SetProperty(ref _actionName, value); }
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
        /// Sets an BattleAbility's ability object using the ability service object
        /// for lookups. 
        /// </summary>
        public ICommand SetCommand { get; set; }

        /// <summary>
        /// Returns the currently selected list.
        /// </summary>
        private ObservableCollection<BattleAbility> SelectedList
        {
            get
            {
                var selectedList = new ObservableCollection<BattleAbility>();

                if (StartSelected)
                {
                    return StartList;
                }
                if (BattleSelected)
                {
                    return BattleList;
                }
                if (EndSelected)
                {
                    return EndList;
                }
                return PullSelected ? PullList : new ObservableCollection<BattleAbility>();
            }
        }

        /// <summary>
        /// Add an move to the currently selected list.
        /// </summary>
        /// <param name="obj"></param>
        private void AddAction(object obj)
        {
            this.SelectedList.Add(new BattleAbility());
        }

        /// <summary>
        /// Remove an move from the currently selected list.
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteAction()
        {
            // Check if user selected an object. 
            if (SelectedIndex >= 0)
            {
                SelectedList.RemoveAt(SelectedIndex);
                // Ensure there is atleast one ability. 
                KeepOne();
            }
        }

        /// <summary>
        /// Clear the currently selected list.
        /// </summary>
        private void ClearActions()
        {
            SelectedList.Clear();

            // Ensure there is atleast one ability. 
            KeepOne();
        }

        /// <summary>
        /// Ensures a list has at least one ability item in it. 
        /// </summary>
        public void KeepOne()
        {
            // Ensure there is atleast one ability. 
            if (!SelectedList.Any())
                this.SelectedList.Add(new BattleAbility());
        }
    }
}
