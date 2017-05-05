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

using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace EasyFarm.Classes
{
    public class BattleList : BindableBase
    {
        private string _name;
        private ObservableCollection<BattleAbility> _value;

        public BattleList()
        {
        }

        public BattleList(string name)
        {
            _name = name;
            _value = new ObservableCollection<BattleAbility> {new BattleAbility()};
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public ObservableCollection<BattleAbility> Actions
        {
            get {
                return _value;
            }
            set { SetProperty(ref _value, value); }
        }
    }
}