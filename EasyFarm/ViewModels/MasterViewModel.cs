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

using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using EasyFarm.Classes;
using EasyFarm.Infrastructure;
using EasyFarm.Logging;
using EasyFarm.Views;
using Application = System.Windows.Application;
using Prism.Commands;
using Prism.Events;

namespace EasyFarm.ViewModels
{
    /// <summary>
    ///     The view model for the main window.
    /// </summary>
    public class MasterViewModel : ViewModelBase
    {
        /// <summary>
        ///     The path of the icon file.
        /// </summary>
        private const string TrayIconFileName = "trayicon.ico";

        /// <summary>
        ///     Saves and loads settings from file.
        /// </summary>
        private readonly SettingsManager _settingsManager;

        /// <summary>
        ///     This program's icon file.
        /// </summary>
        private readonly NotifyIcon _trayIcon = new NotifyIcon();

        /// <summary>
        ///     The text displayed on the start / pause button.
        /// </summary>
        private string _startStopHeader = "St_art";

        public MasterViewModel()
        {
            // Create a new settings manager and associate it with our
            // .eup file type. 
            _settingsManager = new SettingsManager("eup","EasyFarm User Preference");

            // Get events from view models to update the status bar's text.
            AppServices.RegisterEvent<Events.StatusBarEvent>(e => StatusBarText = e.Message);
            AppServices.RegisterEvent<Events.PauseEvent>(x => StopEngine());
            AppServices.RegisterEvent<Events.ResumeEvent>(x => StartEngine());

            // Bind commands to their handlers. 
            StartCommand = new DelegateCommand(Start);
            ExitCommand = new DelegateCommand(Exit);
            SaveCommand = new DelegateCommand(Save);
            LoadCommand = new DelegateCommand(Load);
            SelectProcessCommand = new DelegateCommand(SelectProcess);

            // Hook up our trayicon for minimization to system tray 
            if (File.Exists(TrayIconFileName))
            {
                _trayIcon.Icon = new Icon(TrayIconFileName);
                MasterView.View.StateChanged += OnStateChanged;
                _trayIcon.Click += TrayIcon_Click;
            }
        }

        /// <summary>
        ///     Bind for the title bar's text.
        /// </summary>
        public string MainWindowTitle
        {
            get { return Config.Instance.MainWindowTitle; }
            set { SetProperty(ref Config.Instance.MainWindowTitle, value); }
        }

        /// <summary>
        ///     Binding for the status bar's text.
        /// </summary>
        public string StatusBarText
        {
            get { return Config.Instance.StatusBarText; }
            set { SetProperty(ref Config.Instance.StatusBarText, value); }
        }

        /// <summary>
        ///     Tells whether the bot is working or not.
        /// </summary>
        public bool IsWorking
        {
            get { return GameEngine.IsWorking; }
            set { SetProperty(ref GameEngine.IsWorking, value); }
        }

        /// <summary>
        ///     Binding for File -> Start/Pause text.
        /// </summary>
        public string StartPauseHeader
        {
            get { return _startStopHeader; }
            set { SetProperty(ref _startStopHeader, value); }
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
        public DelegateCommand SaveCommand { get; set; }

        /// <summary>
        ///     Command to load the user's settings.
        /// </summary>
        public DelegateCommand LoadCommand { get; set; }

        /// <summary>
        ///     Binding for selecting a PlayOnline process.
        /// </summary>
        public DelegateCommand SelectProcessCommand { get; set; }

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
            Logger.Write.BotStart("Bot now running");
            AppServices.InformUser("Program running.");
            StartPauseHeader = "P_ause";
            GameEngine.Start();
        }

        private void StopEngine()
        {
            Logger.Write.BotStop("Bot now paused");
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
                Logger.Write.SaveSettings("Settings saved");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                AppServices.InformUser("Failed to save settings.");
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
                Logger.Write.SaveSettings("Settings loaded");
            }
            catch (InvalidOperationException)
            {
                AppServices.InformUser("Failed to load settings.");
            }
        }

        /// <summary>
        ///     Selects a process to user for this application.
        /// </summary>
        private void SelectProcess()
        {
            // Let user select ffxi process
            var selectionView = new ProcessSelectionView();
            selectionView.ShowDialog();

            // Grab the view model with the game sessions. 
            var viewModel = selectionView.DataContext as ProcessSelectionViewModel;

            // If the view has a process selection view model binded to it. 
            if (viewModel != null)
            {
                // Get the selected process. 
                var process = viewModel.SelectedProcess;

                // User never selected a process. 
                if (process == null || !viewModel.IsProcessSelected)
                {
                    Logger.Write.ProcessNotFound("Process not found");
                    AppServices.InformUser("No valid process was selected.");
                    return;
                }

                // Log that a process selected. 
                Logger.Write.ProcessFound("Process found");

                // Get memory reader set in config file. 
                var fface = MemoryWrapper.Create(process.Id);

                // Set the fface Session. 
                SetSession(fface);

                // Tell the user the program has loaded the player's data
                AppServices.InformUser("Bot Loaded: " + fface.Player.Name);

                // Set the main window's title to the player's name. 
                MainWindowTitle = "EasyFarm - " + fface.Player.Name;
            }
        }

        /// <summary>
        ///     Shutsdown the program.
        /// </summary>
        private void Exit()
        {
            Application.Current.Shutdown();
        }

        /* 
         * View Specific Data 
         * We should refactor this out eventually, but it's better to have the code here than
         * in the code behind. This makes it a little bit easier to swap views out. 
         */

        /// <summary>
        ///     Shows the main window when we click this icon in the system tray.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TrayIcon_Click(object sender, EventArgs e)
        {
            MasterView.View.WindowState = WindowState.Normal;
            MasterView.View.ShowInTaskbar = true;
            _trayIcon.Visible = false;
        }

        /// <summary>
        ///     Minimizes the application to the system tray.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnStateChanged(object sender, EventArgs e)
        {
            // Perform tray icon information update here to 
            // receive current title bar information. 
            _trayIcon.Text = MainWindowTitle;
            _trayIcon.BalloonTipText = "EasyFarm has been minimized. ";
            _trayIcon.BalloonTipTitle = MainWindowTitle;

            if (MasterView.View.MnuMinimizeToTray.IsChecked && MasterView.View.WindowState == WindowState.Minimized)
            {
                _trayIcon.Visible = true;
                _trayIcon.ShowBalloonTip(30);
                MasterView.View.ShowInTaskbar = false;
            }
        }
    }
}