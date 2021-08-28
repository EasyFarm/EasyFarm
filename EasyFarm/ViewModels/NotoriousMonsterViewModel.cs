// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013 Mykezero
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
using System.Collections.ObjectModel;
using EasyFarm.Classes;
using EasyFarm.UserSettings;

namespace EasyFarm.ViewModels
{
    public class NotoriousMonsterViewModel : ListViewModel<string>
    {
        public NotoriousMonsterViewModel()
        {
            ViewName = "NotoriousMonster";
        }


        public bool IsEnabled
        {
            get { return Config.Instance.IsNMHunting; }
            set { Set(ref Config.Instance.IsNMHunting, value); }
        }

        public string PlaceholderID
        {
            get { return Config.Instance.PlaceholderID; }
            set { Set(ref Config.Instance.PlaceholderID, value); }
        }

        public string Name
        {
            get { return Config.Instance.NotoriousMonsterName; }
            set { Set(ref Config.Instance.NotoriousMonsterName, value); }
        }
    }
}