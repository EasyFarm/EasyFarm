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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EasyFarm.Infrastructure;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace EasyFarm.ViewModels
{
    public class SelectProcessViewModel : ViewModelBase
    {
        /// <summary>
        ///     The name of the Processes to search for.
        /// </summary>
        private const string ProcessName = "pol";

        public SelectProcessViewModel(BaseMetroDialog dialog)
        {
            // When window is closed through X button. 
            Processes = new ObservableCollection<Process>();

            // Close window on when "Set character" is pressed. 
            SelectCommand = new RelayCommand(async () => await OnSelect());
            RefreshCommand = new RelayCommand(OnRefresh);

            OnRefresh();
            Dialog = dialog;
        }

        /// <summary>
        ///     If the user has selected a Processes.
        /// </summary>
        public bool IsProcessSelected { get; set; }

        /// <summary>
        ///     Toggles whether the program show only pol.exe processes or
        ///     all processes (in case they are targeting a private server).
        /// </summary>
        public RelayCommand RefreshCommand { get; set; }

        /// <summary>
        ///     The currently selected game session.
        /// </summary>
        public Process SelectedProcess { get; set; }

        /// <summary>
        ///     Makes the binded window exit.
        /// </summary>
        public RelayCommand SelectCommand { get; set; }

        public ObservableCollection<Process> Processes { get; set; }

        public BaseMetroDialog Dialog { get; }

        /// <summary>
        /// Refresh the processes. 
        /// </summary>
        private void OnRefresh()
        {
            Processes.Clear();

            Processes.AddRange(Process.GetProcessesByName(ProcessName)
                .Where(x => string.IsNullOrWhiteSpace((x.MainWindowTitle)))
                .ToList());

            if(Processes.Count > 0) return;

            Processes.AddRange(Process.GetProcesses()
                .Where(x => !string.IsNullOrWhiteSpace(x.MainWindowTitle))
                .ToList());
        }

        /// <summary>
        ///     Cleans up Processes watcher resources.
        /// </summary>
        private async Task OnSelect()
        {
            // User made a choice to close this dialog. 
            IsProcessSelected = true;
            await DialogCoordinator.Instance.HideMetroDialogAsync(App.Current.MainWindow.DataContext, Dialog);
        }
    }

    public static class LinqExtensions
    {
        public static ICollection<T> AddRange<T>(this ICollection<T> source, IEnumerable<T> addSource)
        {
            foreach (T item in addSource)
            {
                source.Add(item);
            }

            return source;
        }
    }
}