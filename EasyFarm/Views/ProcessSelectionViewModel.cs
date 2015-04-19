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
        /// The default name of the retail client's executable. 
        /// </summary>
        private const string PROCESS_NAME = "pol";

        /// <summary>
        /// Monitors processes entering and leaving the system. 
        /// </summary>
        private ProcessWatcher _processWatcher;

        /// <summary>
        /// List of running game sessions. 
        /// </summary>
        public ObservableCollection<Process> Sessions { get; set; }

        /// <summary>
        /// Internal backing for the toggle button header. 
        /// </summary>
        private string _toggleButtonHeader;

        /// <summary>
        /// The current header of the toggle process button. 
        /// </summary>
        public string ToggleButtonHeader
        {
            get { return _toggleButtonHeader; }
            set { SetProperty(ref _toggleButtonHeader, value); }
        }

        /// <summary>
        /// The currently selected game session. 
        /// </summary>
        public Process SelectedProcess { get; set; }

        /// <summary>
        /// If the user has selected a process. 
        /// </summary>
        public bool IsProcessSelected { get; set; }

        /// <summary>
        /// The name of the process to search for. 
        /// </summary>
        public string ProcessName = PROCESS_NAME;

        public ProcessSelectionViewModel()
        {
            Sessions = new ObservableCollection<Process>();
            ToggleButtonHeader = "Show All";

            // Create and start a new process watcher to monitor processes. 
            _processWatcher = new ProcessWatcher(ProcessName);
            _processWatcher.Entry += SessionEntry;
            _processWatcher.Exit += SessionExit;
            _processWatcher.Start();

            ExitCommand = new DelegateCommand(OnClosing);
            ToggleFiltering = new DelegateCommand(ChangeFilter);
        }

        /// <summary>
        /// Toggles whether we should show only pol.exe processes or 
        /// all processes. 
        /// </summary>
        private void ChangeFilter()
        {
            // Toggle the process name. The process watcher knows that 
            // the empty string means locate all processes. 
            if (ProcessName.Equals(PROCESS_NAME))
            {
                ProcessName = string.Empty;
                ToggleButtonHeader = "POL Only";
            }
            else
            {
                ProcessName = PROCESS_NAME;
                ToggleButtonHeader = "Show All";
            }

            // Dispose of the old watcher. 
            _processWatcher.Stop();
            _processWatcher.Dispose();

            // Clear all previously found processes.
            Sessions.Clear();

            // Start up the new watcher. 
            _processWatcher = new ProcessWatcher(ProcessName);
            _processWatcher.Entry += SessionEntry;
            _processWatcher.Exit += SessionExit;
            _processWatcher.Start();
        }

        /// <summary>
        /// Toggles whether the program show only pol.exe processes or 
        /// all processes (in case they are targeting a private server).
        /// </summary>
        public DelegateCommand ToggleFiltering { get; set; }

        /// <summary>
        /// Makes the binded window exit. 
        /// </summary>
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

            // Remove the process from our sessions. 
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

            // Add the process to our sessions. 
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
            // Dispose of the running process watcher. 
            _processWatcher.Stop();
            _processWatcher.Dispose();

            // Close our window. 
            foreach (Window window in App.Current.Windows)
            {
                var viewModel = window.DataContext as ProcessSelectionViewModel;
                if (viewModel == null) continue;
                window.Close();
            }

            // User made a choice to close this dialog. 
            IsProcessSelected = true;
        }
    }
}
