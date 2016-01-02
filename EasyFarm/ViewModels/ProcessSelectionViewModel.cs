using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using EasyFarm.Views;
using Prism.Commands;
using Prism.Mvvm;

namespace EasyFarm.ViewModels
{
    public class ProcessSelectionViewModel : BindableBase
    {
        /// <summary>
        ///     The name of the Processes to search for.
        /// </summary>
        private const string ProcessName = "pol";

        private readonly ProcessSelectionView _view;

        public ProcessSelectionViewModel(ProcessSelectionView view)
        {
            // When window is closed through X button. 
            _view = view;
            Processes = new ObservableCollection<Process>();

            // Close window on when "Set character" is pressed. 
            SelectCommand = new DelegateCommand(OnSelect);
            RefreshCommand = new DelegateCommand(OnRefresh);

            OnRefresh();
        }

        /// <summary>
        ///     If the user has selected a Processes.
        /// </summary>
        public bool IsProcessSelected { get; set; }

        /// <summary>
        ///     Toggles whether the program show only pol.exe processes or
        ///     all processes (in case they are targeting a private server).
        /// </summary>
        public DelegateCommand RefreshCommand { get; set; }

        /// <summary>
        ///     The currently selected game session.
        /// </summary>
        public Process SelectedProcess { get; set; }

        /// <summary>
        ///     Makes the binded window exit.
        /// </summary>
        public DelegateCommand SelectCommand { get; set; }

        public ObservableCollection<Process> Processes { get; set; }

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
        private void OnSelect()
        {
            // Close the character selection screen. 
            _view.Close();

            // User made a choice to close this dialog. 
            IsProcessSelected = true;
        }
    }
}