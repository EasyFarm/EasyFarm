
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

using EasyFarm.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    public class ActionInfo
    {
        public ActionInfo()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            BattleList = new ObservableCollection<Ability>();
            StartList = new ObservableCollection<Ability>();
            EndList = new ObservableCollection<Ability>();
            HealingList = new ObservableCollection<ListItem<HealingAbility>>();

            StartListSelected = true;
            BattleListSelected = false;
            EndListSelected = false;
        }

        /// <summary>
        /// List of actions that should be used before battle
        /// </summary>
        public ObservableCollection<Ability> StartList { get; set; }

        /// <summary>
        /// List of actions taht should be used at the end of battle
        /// </summary>
        public ObservableCollection<Ability> EndList { get; set; }

        /// <summary>
        /// List of actions that should be used in battle
        /// </summary>
        public ObservableCollection<Ability> BattleList { get; set; }

        /// <summary>
        /// List of actions that should be used when injured
        /// </summary>
        public ObservableCollection<ListItem<HealingAbility>> HealingList { get; set; }

        /// <summary>
        /// Is the BattleList Selected in the battle tab?
        /// </summary>
        public bool BattleListSelected { get; set; }

        /// <summary>
        /// Is the StartList selected in the battle tab?
        /// </summary>
        public bool StartListSelected { get; set; }

        /// <summary>
        /// Is the End list selected in the battle tab?
        /// </summary>
        public bool EndListSelected { get; set; }

        /// <summary>
        /// The name of the ability going to be added to either the Battle/Start/End Action Lists
        /// </summary>
        public string BattleActionName { get; set; }                          
    }
}
