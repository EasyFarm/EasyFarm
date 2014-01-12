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
        public ObservableCollection<AbilityListItem<HealingAbility>> HealingList
        {
            get { return Engine.Config.HealingList; }
            set { Engine.Config.HealingList = value; }
        }

        public ICommand AddHealingCommand
        {
            get
            {
                return new RelayCommand(AddHealingItem);
            }
        }

        public ICommand DeleteHealingCommand
        {
            get
            {
                return new RelayCommand(DeleteHealing);
            }
        }

        public ICommand ClearHealingCommand
        {
            get
            {
                return new RelayCommand(ClearHealing);
            }
        }

        private void ClearHealing(object obj)
        {
            HealingList.Clear();
        }

        private void DeleteHealing(object obj)
        {
            HealingList.Remove(obj as AbilityListItem<HealingAbility>);
        }

        private void AddHealingItem(object obj)
        {
            HealingList.Add(new AbilityListItem<HealingAbility>(new HealingAbility() { IsEnabled = false, Name = "Empty", TriggerLevel = 0}));
        }
    }
}
