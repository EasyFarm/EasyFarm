using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Classes
{
    public class BattleList : BindableBase
    {
        public BattleList() { }

        public BattleList(string name)
        {
            _name = name;
            _actions = new ObservableCollection<BattleAbility>();
            _actions.Add(new BattleAbility() { Name = "Default" });
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private ObservableCollection<BattleAbility> _actions;

        public ObservableCollection<BattleAbility> Actions
        {
            get { return _actions; }
            set { SetProperty(ref _actions, value); }
        }
    }
}
