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
