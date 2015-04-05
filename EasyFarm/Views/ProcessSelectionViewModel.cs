using EasyFarm.Classes;
using EasyFarm.Logging;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EasyFarm.ViewModels
{
    public class ProcessSelectionViewModel : BindableBase
    {
        /// <summary>
        /// Monitors processes entering and leaving the system. 
        /// </summary>
        private ProcessWatcher _processWatcher = new ProcessWatcher("pol");

        /// <summary>
        /// List of running game sessions. 
        /// </summary>
        public ObservableCollection<Process> Sessions { get; set; }
        
        public ProcessSelectionViewModel()
        {
            Sessions = new ObservableCollection<Process>();

            _processWatcher.Entry += SessionEntry;
            _processWatcher.Exit += SessionExit;
            _processWatcher.Start();

            ExitCommand = new DelegateCommand(OnClosing);
        }

        public DelegateCommand ExitCommand { get; set; }

        /// <summary>
        /// Event that fires when processes exit the system. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SessionExit(object sender, EventArgs e)
        {
            // Checks for the app class for null since if the program is exiting
            // it may not be available. 
            if (App.Current == null) return;
            if (App.Current.Dispatcher == null) return;

            App.Current.Dispatcher.Invoke(() =>
            {
                var process = (e as ProcessEventArgs).Process;
                if (process == null) return;
                Sessions.Remove(process);
            });
        }

        /// <summary>
        /// Event that fires when processes enters the system. We add the new processes 
        /// to our internal list for a view to bind to. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SessionEntry(object sender, EventArgs e)
        {
            // Checks for the app class for null since if the program is exiting
            // it may not be available. 
            if (App.Current == null) return;
            if (App.Current.Dispatcher == null) return;

            App.Current.Dispatcher.Invoke(() =>
            {
                var process = (e as ProcessEventArgs).Process;
                if (process == null) return;
                Sessions.Add(process);
            });
        }

        /// <summary>
        /// Cleans up process watcher resources. 
        /// </summary>
        private void OnClosing()
        {
            _processWatcher.Stop();
            _processWatcher.Dispose();

            // Close our window. 
            foreach (Window window in App.Current.Windows)
            {
                var viewModel = window.DataContext as ProcessSelectionViewModel;
                if (viewModel == null) continue;
                window.Close();
            }
        }
    }
}
