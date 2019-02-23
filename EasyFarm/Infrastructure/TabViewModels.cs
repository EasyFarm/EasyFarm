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
using System;
using System.Collections.Generic;
using System.Linq;
using EasyFarm.ViewModels;

namespace EasyFarm.Infrastructure
{
    public class TabViewModels
    {
        public IList<IViewModel> ViewModels = new List<IViewModel>();

        public TabViewModels(IList<IViewModel> viewModels)
        {
            ViewModels = viewModels;
            ViewModels = ViewModels
                .Where(x => AvailableTabs.ContainsKey(x.GetType()))
                .OrderBy(x => AvailableTabs[x.GetType()])
                .ToList();
        }

        /// <summary>
        /// The available tabs in the main view with their given display order.
        /// </summary>
        public Dictionary<Type, int> AvailableTabs => new Dictionary<Type, int>
        {
            { typeof(BattlesViewModel), 1 },
            { typeof(TargetingViewModel), 2 },
            { typeof(RestingViewModel), 3 },
            { typeof(RoutesViewModel), 4 },
            { typeof(FollowViewModel), 5 },
            { typeof(LogViewModel), 6 },
            { typeof(SettingsViewModel), 7 }
        };
    }
}