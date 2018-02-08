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

using System.Collections.ObjectModel;
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

        public MainViewModel(TabViewModels tableViewModel)
        {
            // Get all enabled view models.
            ViewModels = new ObservableCollection<IViewModel>(tableViewModel.ViewModels);
        }

        /// <summary>
        ///     List of dynamically found view models.
        /// </summary>
        public ObservableCollection<IViewModel> ViewModels
        {
            get { return _viewModels; }
            set { Set(ref _viewModels, value); }
        }

        /// <summary>
        ///     Index for the currently focused tab.
        /// </summary>
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { Set(ref _selectedIndex, value); }
        }
    }
}