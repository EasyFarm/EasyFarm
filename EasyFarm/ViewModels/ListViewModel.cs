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
using System.Windows.Input;
using EasyFarm.Infrastructure;
using GalaSoft.MvvmLight.Command;

namespace EasyFarm.ViewModels
{
    /// <summary>
    /// View model for list controls.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ListViewModel<T> : ViewModelBase
    {
        /// <summary>
        /// The current value to be added (Textboxes) or has been selected (ListControls)
        /// </summary>
        public virtual T Value { get; set; }

        /// <summary>
        /// The values of the list control.
        /// </summary>
        public virtual ObservableCollection<T> Values { get; set; }

        public ListViewModel()
        {
            AddCommand = new RelayCommand(Add);
            DeleteCommand = new RelayCommand(Delete);
            ClearCommand = new RelayCommand(Clear);
        }

        /// <summary>
        /// Add command for view binding.
        /// </summary>
        public ICommand AddCommand { get; set; }

        /// <summary>
        /// Delete command for view binding.
        /// </summary>
        public ICommand DeleteCommand { get; set; }

        /// <summary>
        /// Clear command for view binding .
        /// </summary>
        public ICommand ClearCommand { get; set; }

        /// <summary>
        /// Clear the items in the list control.
        /// </summary>
        protected virtual void Clear()
        {
            Values.Clear();
        }

        /// <summary>
        /// Delete existing items in the list control.
        /// </summary>
        protected virtual void Delete()
        {
            if (!Values.Contains(Value)) return;
            Values.Remove(Value);
        }

        /// <summary>
        /// Add a new value to the list control.
        /// </summary>
        protected virtual void Add()
        {
            if (Values.Contains(Value)) return;
            Values.Add(Value);
        }
    }
}