
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

using EasyFarm.Classes;
using EasyFarm.UserSettings;
using Microsoft.Practices.Prism.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace EasyFarm.ViewModels
{
    [ViewModelAttribute("Battles")]
    public class BattlesViewModel : ViewModelBase
    {
        public BattlesViewModel()
        {
            AddActionCommand = new DelegateCommand(AddAction);
            DeleteActionCommand = new DelegateCommand(DeleteAction);
            ClearActionsCommand = new DelegateCommand(ClearActions);
            
        }

        public int SelectedIndex { get; set; }

        /// <summary>
        /// The selected in the currently selected list. 
        /// </summary>
        public BattleAbility SelectedAbility
        {
            get
            {
                if (SelectedIndex < 0)
                {
                    return new BattleAbility();
                }
                else
                {
                    return this.SelectedList[SelectedIndex];
                }
            }
        }

        /// <summary>
        /// The list of moves to use before battle.
        /// </summary>
        public ObservableCollection<BattleAbility> StartList
        {
            get { return Config.Instance.StartList; }
            set { SetProperty(ref Config.Instance.StartList, value); }
        }

        /// <summary>
        /// The list of moves to use during battle.
        /// </summary>
        public ObservableCollection<BattleAbility> BattleList
        {
            get { return Config.Instance.BattleList; }
            set { SetProperty(ref Config.Instance.BattleList, value); }
        }

        /// <summary>
        /// The list of moves to use after battle.
        /// </summary>
        public ObservableCollection<BattleAbility> EndList
        {
            get { return Config.Instance.EndList; }
            set { SetProperty(ref Config.Instance.EndList, value); }

        }

        public ObservableCollection<BattleAbility> PullList
        {
            get { return Config.Instance.PullList; }
            set { SetProperty(ref Config.Instance.PullList, value); }
        }

        /// <summary>
        /// When selected displays the battle moves list.
        /// </summary>
        public bool BattleSelected
        {
            get { return Config.Instance.BattleListSelected; }
            set
            {
                SetProperty(ref Config.Instance.BattleListSelected, value);
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
            get { return Config.Instance.StartListSelected; }
            set
            {
                SetProperty(ref Config.Instance.StartListSelected, value);
                KeepOne();
            }
        }

        /// <summary>
        /// When selected displays the ending moves list.
        /// </summary>
        public bool EndSelected
        {
            get { return Config.Instance.EndListSelected; }
            set
            {
                SetProperty(ref Config.Instance.EndListSelected, value);
                KeepOne();
            }
        }

        /// <summary>
        /// When selected displays the ending moves list.
        /// </summary>
        public bool PullSelected
        {
            get { return Config.Instance.PullListSelected; }
            set
            {
                SetProperty(ref Config.Instance.PullListSelected, value);
                KeepOne();
            }
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
        private void AddAction()
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

namespace EasyFarm.UserSettings
{
    public partial class Config
    {
        /// <summary>
        /// List of actions that should be used before battle
        /// </summary>
        public ObservableCollection<BattleAbility> StartList =
            new ObservableCollection<BattleAbility>();

        /// <summary>
        /// List of actions taht should be used at the end of battle
        /// </summary>
        public ObservableCollection<BattleAbility> EndList =
            new ObservableCollection<BattleAbility>();

        /// <summary>
        /// List of actions that should be used in battle
        /// </summary>
        public ObservableCollection<BattleAbility> BattleList =
            new ObservableCollection<BattleAbility>();

        /// <summary>
        /// List of moves that should be used to pull a creature. 
        /// </summary>
        public ObservableCollection<BattleAbility> PullList =
            new ObservableCollection<BattleAbility>();

        /// <summary>
        /// Is the BattleList Selected in the battle tab?
        /// </summary>
        public bool BattleListSelected = true;

        /// <summary>
        /// Is the StartList selected in the battle tab?
        /// </summary>
        public bool StartListSelected = false;

        /// <summary>
        /// Is the Pulling list selected in the battle tab?
        /// </summary>
        public bool PullListSelected = false;

        /// <summary>
        /// Is the End list selected in the battle tab?
        /// </summary>
        public bool EndListSelected = false;
    }
}
