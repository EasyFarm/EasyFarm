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

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using EasyFarm.Classes;
using EasyFarm.Infrastructure;
using EasyFarm.Logging;
using EasyFarm.Persistence;
using EasyFarm.UserSettings;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls.Dialogs;
using MediatR;
using Application = System.Windows.Application;

namespace EasyFarm.ViewModels
{
    /// <summary>
    ///     The view model for the main window.
    /// </summary>
    public class MasterViewModel : ViewModelBase
    {
        private readonly IMediator _mediator;
        private readonly LibraryUpdater _updater;
        private readonly IDialogCoordinator _dialogCoordinator;

        /// <summary>
        ///     Saves and loads settings from file.
        /// </summary>
        private readonly SettingsManager _settingsManager;

        /// <summary>
        ///     The text displayed on the start / pause button.
        /// </summary>
        private string _startStopHeader = "St_art";

        /// <summary>
        /// The view's main body content.
        /// </summary>
        public IViewModel ViewModel { get; set; }

        public MasterViewModel(
            MainViewModel mainViewModel, 
            IMediator mediator, 
            LibraryUpdater updater,
            IDialogCoordinator dialogCoordinator)
        {
            _mediator = mediator;
            _updater = updater;
            _dialogCoordinator = dialogCoordinator;
            ViewModel = mainViewModel;

            _settingsManager = new SettingsManager("eup", "EasyFarm User Preference");

            AppServices.RegisterEvent<Events.TitleEvent>(this, e => MainWindowTitle = e.Message);
            AppServices.RegisterEvent<Events.StatusBarEvent>(this, e => StatusBarText = e.Message);
            AppServices.RegisterEvent<Events.PauseEvent>(this, x => StopEngine());
            AppServices.RegisterEvent<Events.ResumeEvent>(this, x => StartEngine());

            StartCommand = new RelayCommand(Start);
            ExitCommand = new RelayCommand(Exit);
            SaveCommand = new RelayCommand(Save);
            LoadCommand = new RelayCommand(Load);
            SelectProcessCommand = new RelayCommand(async () => await SelectProcess());
            LoadedCommand = new RelayCommand(async () => await OnLoad());
        }

        private string _mainWindowTitle;

        public string MainWindowTitle
        {
            get { return _mainWindowTitle; }
            set { Set(ref _mainWindowTitle, value); }
        }

        private string _statusBarText;

        public string StatusBarText
        {
            get { return _statusBarText; }
            set { Set(ref _statusBarText, value); }
        }

        /// <summary>
        ///     Tells whether the bot is working or not.
        /// </summary>
        public bool IsWorking
        {
            get { return GameEngine.IsWorking; }
            set { Set(ref GameEngine.IsWorking, value); }
        }

        /// <summary>
        ///     Binding for File -> Start/Pause text.
        /// </summary>
        public string StartPauseHeader
        {
            get { return _startStopHeader; }
            set { Set(ref _startStopHeader, value); }
        }

        public bool MinimizeToTray
        {
            get { return Config.Instance.MinimizeToTray; }
            set
            {
                bool instanceMinimizeToTray = false;
                Set(ref instanceMinimizeToTray, value);
                Config.Instance.MinimizeToTray = instanceMinimizeToTray;
            }
        }

        /// <summary>
        ///     Command to start the bot.
        /// </summary>
        public ICommand StartCommand { get; set; }

        /// <summary>
        ///     Command to shut down the program.
        /// </summary>
        public ICommand ExitCommand { get; set; }

        /// <summary>
        ///     Command to save the user's settings.
        /// </summary>
        public RelayCommand SaveCommand { get; set; }

        /// <summary>
        ///     Command to load the user's settings.
        /// </summary>
        public RelayCommand LoadCommand { get; set; }

        /// <summary>
        ///     Binding for selecting a PlayOnline process.
        /// </summary>
        public RelayCommand SelectProcessCommand { get; set; }

        /// <summary>
        /// Invoked when the view is loaded.
        /// </summary>
        public RelayCommand LoadedCommand { get; }

        /// <summary>
        ///     Tells the program to start farming.
        /// </summary>
        public void Start()
        {
            // Return when the user has not selected a process.
            if (FFACE == null)
            {
                AppServices.InformUser("No process has been selected.");
                return;
            }

            if (GameEngine.IsWorking)
            {
                StopEngine();
            }
            else
            {
                StartEngine();
            }
        }

        private void StartEngine()
        {
            LogViewModel.Write("Bot now running");
            AppServices.InformUser("Program running.");
            var isStarted = GameEngine.Start();
            if (!isStarted) return;
            StartPauseHeader = "P_ause";
        }

        private void StopEngine()
        {
            LogViewModel.Write("Bot now paused");
            AppServices.InformUser("Program paused.");
            StartPauseHeader = "St_art";
            GameEngine.Stop();
        }

        /// <summary>
        ///     Saves the player's data to file.
        /// </summary>
        private void Save()
        {
            try
            {
                _settingsManager.TrySave(Config.Instance);
                AppServices.InformUser("Settings have been saved.");
                LogViewModel.Write("Settings saved");
            }
            catch (InvalidOperationException ex)
            {
                AppServices.InformUser("Failed to save settings.");
                Logger.Log(new LogEntry(LoggingEventType.Error, $"{GetType()}.{nameof(Save)} : Failure on save settings", ex));
            }
        }

        /// <summary>
        ///     Loads easyfarm settings from file.
        /// </summary>
        private void Load()
        {
            try
            {
                // Load the settings.
                var settings = _settingsManager.TryLoad<Config>();

                // Did we fail to load the settings?
                if (settings == null)
                {
                    AppServices.InformUser("Failed to load settings.");
                    return;
                }

                // Inform the user of our success.
                Config.Instance = settings;
                AppServices.InformUser("Settings have been loaded.");
                LogViewModel.Write("Settings loaded");
            }
            catch (InvalidOperationException ex)
            {
                AppServices.InformUser("Failed to load settings.");
                Logger.Log(new LogEntry(LoggingEventType.Error, $"{GetType()}.{nameof(Load)} : Failed to load settings", ex));
            }
        }

        /// <summary>
        ///     Selects a process to user for this application.
        /// </summary>
        private async Task SelectProcess()
        {
            await _mediator.Send(new SelectProcessRequest());
        }

        /// <summary>
        ///     Shutsdown the program.
        /// </summary>
        private void Exit()
        {
            Application.Current.Shutdown();
        }

        public async Task OnLoad()
        {
            MainWindowTitle = "EasyFarm";
            StatusBarText = "";

            if (_updater.HasUpdate())
            {
                var showDialogResult = await _dialogCoordinator.ShowMessageAsync(
                    Application.Current.MainWindow.DataContext,
                    "Update EliteAPI",
                    "Would you like to update EliteAPI to its newest version?",
                    MessageDialogStyle.AffirmativeAndNegative);

                if (showDialogResult == MessageDialogResult.Affirmative)
                    _updater.Update();
            }
        }
    }
}
