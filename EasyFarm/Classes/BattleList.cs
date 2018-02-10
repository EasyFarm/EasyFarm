// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
// 
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
using EasyFarm.Infrastructure;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;


namespace EasyFarm.Classes
{
    public class BattleList : ObservableObject
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
            set { Set(ref _name, value); }
        }

        public ObservableCollection<BattleAbility> Actions
        {
            get {
                return _value;
            }
            set { Set(ref _value, value); }
        }
    }
}