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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EasyFarm.Infrastructure;

namespace EasyFarm.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        ///     Interal stating index for the currently focused tab.
        /// </summary>
        private int _selectedIndex;

        /// <summary>
        /// 
        ///     Internal list of view models.
        /// </summary>
        private ObservableCollection<IViewModel> _viewModels;

        public MainViewModel()
        {
            // Get all enabled view models. 
            ViewModels = new ObservableCollection<IViewModel>(
                new List<IViewModel>
                {
                    new BattlesViewModel(),                    
                    new TargetingViewModel(),
                    new RestingViewModel(),
                    new RoutesViewModel(),
                    new FollowViewModel(),
                    new LogViewModel(),
                    new SettingsViewModel()
                });
        }

        /// <summary>
        ///     List of dynamically found view models.
        /// </summary>
        public ObservableCollection<IViewModel> ViewModels
        {
            get { return _viewModels; }
            set { SetProperty(ref _viewModels, value); }
        }

        /// <summary>
        ///     Index for the currently focused tab.
        /// </summary>
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }
    }
}