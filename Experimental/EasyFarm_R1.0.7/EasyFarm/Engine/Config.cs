using System;
using System.Collections.Generic;
using FFACETools;
using EasyFarm.PlayerTools;
using System.Collections.ObjectModel;
using EasyFarm.UtilityTools;
using EasyFarm.MVVM;

namespace EasyFarm.Engine
{
    /// <summary>
    /// A configuration file for the user to edit through his GUI.
    /// Gives the bot access to allow of his decisions.
    /// </summary>
    public class Config
    {
        private GameEngine m_gameEngine;

        private Config() { }

        public Config(ref GameEngine m_gameEngine)
        {            
            this.m_gameEngine = m_gameEngine;
        }

        public ObservableCollection<Ability> StartList { get; set; }
        public ObservableCollection<Ability> EndList { get; set; }
        public ObservableCollection<Ability> BattleList { get; set; }
        public ObservableCollection<AbilityListItem<HealingAbility>> HealingList { get; set; }

        public ObservableCollection<String> IgnoredList { get; set; }
        public ObservableCollection<String> TargetsList { get; set; }
        public ObservableCollection<FFACE.Position> Waypoints { get; set; }

        public WeaponAbility Weaponskill { get; set; }
        public int WSHealthThreshold { get; set; }

        public bool BattleAggro { get; set; }
        public bool BattlePartyClaimed { get; set; }
        public bool BattleUnclaimed { get; set; }

        // Add default values.
        public string TargetsName { get; set; }
        public string IgnoredName { get; set; }
        public string StatusBarText { get; set; }
        public bool BattleListSelected { get; set; }
        public bool StartListSelected { get; set; }
        public bool EndListSelected { get; set; }
        public string BattleActionName { get; set; }
        public string WSName { get; set; }
        public double WSDistance { get; set; }
        public int LowMP { get; set; }
        public int HighMP { get; set; }
        public int HighHP { get; set; }
        public int LowHP { get; set; }

        public bool IsRestingHPEnabled { get; set; }

        public bool IsRestingMPEnabled { get; set; }
    }
}