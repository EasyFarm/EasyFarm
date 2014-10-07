
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
using System.Collections.ObjectModel;
using ZeroLimits.XITools;

namespace EasyFarm.UserSettings
{
    public class ActionInfo
    {
        /// <summary>
        /// List of actions that should be used before battle
        /// </summary>
        public ObservableCollection<Ability> StartList = new ObservableCollection<Ability>();

        /// <summary>
        /// List of actions taht should be used at the end of battle
        /// </summary>
        public ObservableCollection<Ability> EndList = new ObservableCollection<Ability>();

        /// <summary>
        /// List of actions that should be used in battle
        /// </summary>
        public ObservableCollection<Ability> BattleList = new ObservableCollection<Ability>();

        /// <summary>
        /// List of moves that should be used to pull a creature. 
        /// </summary>
        public ObservableCollection<Ability> PullList = new ObservableCollection<Ability>();

        /// <summary>
        /// List of actions that should be used when injured
        /// </summary>
        public ObservableCollection<HealingAbility> HealingList = new ObservableCollection<HealingAbility>();

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
