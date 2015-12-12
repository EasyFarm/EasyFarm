using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using EasyFarm.Monitors;
using Prism.Mvvm;
using Prism.Commands;
using EasyFarm.Views;

namespace EasyFarm.ViewModels
{
    public class ProcessSelectionViewModel : BindableBase
    {
        /// <summary>
        ///     The default name of the retail client's executable.
        /// </summary>
        private const string ClientName = "pol";

        /// <summary>
        ///     Monitors processes entering and leaving the system.
        /// </summary>
        private ProcessWatcher _processWatcher;

        /// <summary>
        ///     Internal backing for the toggle button header.
        /// </summary>
        private string _toggleButtonHeader;

        /// <summary>
        ///     The name of the process to search for.
        /// </summary>
        public string ProcessName = ClientName;
        private readonly ProcessSelectionView view;

        public ProcessSelectionViewModel(ProcessSelectionView view)
        {
            this.view = view;

            // When window is closed through X button. 
            view.Closing += (s, e) => OnClosing();

            Sessions = new ObservableCollection<Process>();
            ToggleButtonHeader = "Show All";

            // Create and start a new process watcher to monitor processes. 
            _processWatcher = new ProcessWatcher(ProcessName);
            _processWatcher.Entry += SessionEntry;
            _processWatcher.Exit += SessionExit;
            _processWatcher.Start();                       

            // Close window on when "Set character" is pressed. 
            ExitCommand = new DelegateCommand(() => view.Close());            

            ToggleFiltering = new DelegateCommand(ChangeFilter);
        }

        /// <summary>
        ///     List of running game sessions.
        /// </summary>
        public ObservableCollection<Process> Sessions { get; set; }

        /// <summary>
        ///     The current header of the toggle process button.
        /// </summary>
        public string ToggleButtonHeader
        {
            get { return _toggleButtonHeader; }
            set { SetProperty(ref _toggleButtonHeader, value); }
        }

        /// <summary>
        ///     The currently selected game session.
        /// </summary>
        public Process SelectedProcess { get; set; }

        /// <summary>
        ///     If the user has selected a process.
        /// </summary>
        public bool IsProcessSelected { get; set; }

        /// <summary>
        ///     Toggles whether the program show only pol.exe processes or
        ///     all processes (in case they are targeting a private server).
        /// </summary>
        public DelegateCommand ToggleFiltering { get; set; }

        /// <summary>
        ///     Makes the binded window exit.
        /// </summary>
        public DelegateCommand ExitCommand { get; set; }

        /// <summary>
        ///     Toggles whether we should show only pol.exe processes or
        ///     all processes.
        /// </summary>
        private void ChangeFilter()
        {
            // Toggle the process name. The process watcher knows that 
            // the empty string means locate all processes. 
            if (ProcessName.Equals(ClientName))
            {
                ProcessName = string.Empty;
                ToggleButtonHeader = "POL Only";
            }
            else
            {
                ProcessName = ClientName;
                ToggleButtonHeader = "Show All";
            }

            // Dispose of the old watcher. 
            _processWatcher.Stop();

            // Clear all previously found processes.
            Sessions.Clear();

            // Start up the new watcher. 
            _processWatcher = new ProcessWatcher(ProcessName);
            _processWatcher.Entry += SessionEntry;
            _processWatcher.Exit += SessionExit;
            _processWatcher.Start();
        }

        /// <summary>
        ///     Event that fires when processes exit the system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SessionExit(object sender, EventArgs e)
        {
            // Checks for the app class for null since if the program is exiting
            // it may not be available. 
            if (Application.Current == null) return;
            if (Application.Current.Dispatcher == null) return;

            // Remove the process from our sessions. 
            Application.Current.Dispatcher.Invoke(() =>
            {
                var processEventArgs = e as ProcessEventArgs;
                if (processEventArgs != null)
                {
                    var process = processEventArgs.Process;
                    if (process == null) return;
                    Sessions.Remove(process);
                }
            });
        }

        /// <summary>
        ///     Event that fires when processes enters the system. We add the new processes
        ///     to our internal list for a view to bind to.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SessionEntry(object sender, EventArgs e)
        {
            // Checks for the app class for null since if the program is exiting
            // it may not be available. 
            if (Application.Current == null) return;
            if (Application.Current.Dispatcher == null) return;

            // Add the process to our sessions. 
            Application.Current.Dispatcher.Invoke(() =>
            {
                var processEventArgs = e as ProcessEventArgs;
                if (processEventArgs != null)
                {
                    var process = processEventArgs.Process;
                    if (process == null) return;
                    Sessions.Add(process);
                }
            });
        }

        /// <summary>
        ///     Cleans up process watcher resources.
        /// </summary>
        private void OnClosing()
        {
            // Dispose of the running process watcher. 
            _processWatcher.Stop();

            // User made a choice to close this dialog. 
            IsProcessSelected = true;            
        }
    }
}