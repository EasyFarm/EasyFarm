using EasyFarm.MVVM;
using EasyFarm.PlayerTools;
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
        public ObservableCollection<ListItem<HealingAbility>> HealingList
        {
            get { return Engine.Config.HealingList; }
            set { Engine.Config.HealingList = value; }
        }

        public ICommand AddHealingCommand { get; set; }

        public ICommand DeleteHealingCommand { get; set; }

        public ICommand ClearHealingCommand { get; set; }

        private void ClearHealing(object obj)
        {
            HealingList.Clear();
        }

        private void DeleteHealing(object obj)
        {
            HealingList.Remove(obj as ListItem<HealingAbility>);
        }

        private void AddHealingItem(object obj)
        {
            HealingList.Add(new ListItem<HealingAbility>(new HealingAbility() { IsEnabled = false, Name = "Empty", TriggerLevel = 0}));
        }
    }
}
