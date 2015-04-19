
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

using EasyFarm.Classes;
using EasyFarm.Logging;
using EasyFarm.Views;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using NotifyIcon = System.Windows.Forms.NotifyIcon;

namespace EasyFarm.ViewModels
{
    /// <summary>
    /// The view model for the main window. 
    /// </summary>
    public class MasterViewModel : ViewModelBase
    {
        /// <summary>
        /// Saves and loads settings from file. 
        /// </summary>
        private SettingsManager _settingsManager;

        /// <summary>
        /// The path of the icon file. 
        /// </summary>
        private const String TRAY_ICON_FILE_NAME = "trayicon.ico";

        /// <summary>
        /// This program's icon file. 
        /// </summary>
        private NotifyIcon m_trayIcon = new NotifyIcon();

        public MasterViewModel()
        {
            // Create a new settings manager and associate it with our
            // .eup file type. 
            _settingsManager = new SettingsManager(
                "eup",
                "EasyFarm User Preference"
            );

            // Get events from view models to update the status bar's text.
            AppInformer.EventAggregator.GetEvent<StatusBarUpdateEvent>().Subscribe(a => { StatusBarText = a; });

            // Bind commands to their handlers. 
            StartCommand = new DelegateCommand(Start);
            ExitCommand = new DelegateCommand(Exit);
            SaveCommand = new DelegateCommand(Save);
            LoadCommand = new DelegateCommand(Load);
            SelectProcessCommand = new DelegateCommand(SelectProcess);

            // Hook up our trayicon for minimization to system tray 
            if (File.Exists(TRAY_ICON_FILE_NAME))
            {
                m_trayIcon.Icon = new System.Drawing.Icon(TRAY_ICON_FILE_NAME);
                MasterView.View.StateChanged += OnStateChanged;
                m_trayIcon.Click += TrayIcon_Click;            
            }            
        }

        /// <summary>
        /// Bind for the title bar's text. 
        /// </summary>
        public String MainWindowTitle
        {
            get { return Config.Instance.MainWindowTitle; }
            set { SetProperty(ref Config.Instance.MainWindowTitle, value); }
        }

        /// <summary>
        /// Binding for the status bar's text. 
        /// </summary>
        public String StatusBarText
        {
            get { return Config.Instance.StatusBarText; }
            set { SetProperty(ref Config.Instance.StatusBarText, value); }
        }

        /// <summary>
        /// Tells whether the bot is working or not. 
        /// </summary>
        public bool IsWorking
        {
            get { return App.GameEngine.IsWorking; }
            set { SetProperty(ref App.GameEngine.IsWorking, value); }
        }

        /// <summary>
        /// The text displayed on the start / pause button. 
        /// </summary>
        private string _startStopHeader = "St_art";

        /// <summary>
        /// Binding for File -> Start/Pause text.
        /// </summary>
        public string StartPauseHeader
        {
            get { return _startStopHeader; }
            set { SetProperty(ref _startStopHeader, value); }
        }
        
        /// <summary>
        /// Command to start the bot. 
        /// </summary>
        public ICommand StartCommand { get; set; }

        /// <summary>
        /// Command to shut down the program. 
        /// </summary>
        public ICommand ExitCommand { get; set; }

        /// <summary>
        /// Command to save the user's settings. 
        /// </summary>
        public DelegateCommand SaveCommand { get; set; }

        /// <summary>
        /// Command to load the user's settings. 
        /// </summary>
        public DelegateCommand LoadCommand { get; set; }

        /// <summary>
        /// Binding for selecting a PlayOnline process. 
        /// </summary>
        public DelegateCommand SelectProcessCommand { get; set; }

        /// <summary>
        /// Tells the program to start farming. 
        /// </summary>
        public void Start()
        {
            // Return when the user has not selected a process. 
            if (FFACE == null)
            {
                AppInformer.InformUser("No process has been selected.");
                return;
            }

            if (App.GameEngine.IsWorking)
            {
                Logger.Write.BotStop("Bot now paused");
                AppInformer.InformUser("Program paused.");
                App.GameEngine.Stop();
                StartPauseHeader = "St_art";
            }
            else
            {
                Logger.Write.BotStart("Bot now running");
                AppInformer.InformUser("Program running.");
                App.GameEngine.Start();
                StartPauseHeader = "P_ause";
            }
        }

        /// <summary>
        /// Saves the player's data to file. 
        /// </summary>
        private void Save()
        {
            try
            {
                _settingsManager.Save<Config>(Config.Instance);
                AppInformer.InformUser("Settings have been saved.");
                Logger.Write.SaveSettings("Settings saved");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                AppInformer.InformUser("Failed to save settings.");
            }
        }

        /// <summary>
        /// Loads easyfarm settings from file. 
        /// </summary>
        private void Load()
        {
            try
            {
                // Load the settings.
                var settings = _settingsManager.Load<Config>();

                // Did we fail to load the settings?
                if (settings == null)
                {
                    AppInformer.InformUser("Failed to load settings.");
                    return;
                }

                // Inform the user of our success. 
                Config.Instance = settings;
                AppInformer.InformUser("Settings have been loaded.");
                Logger.Write.SaveSettings("Settings loaded");
            }
            catch (InvalidOperationException)
            {
                AppInformer.InformUser("Failed to load settings.");
            }
        }

        /// <summary>
        /// Selects a process to user for this application. 
        /// </summary>
        private void SelectProcess()
        {
            // Let user select ffxi process
            ProcessSelectionView selectionView = new ProcessSelectionView();
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
                    AppInformer.InformUser("No valid process was selected.");
                    return;
                }

                // Log that a process selected. 
                Logger.Write.ProcessFound("Process found");

                // Save the selected fface instance. 
                var FFACE = new FFACETools.FFACE(process.Id);

                // Set the FFACE Session. 
                ViewModelBase.SetSession(FFACE);

                // Tell the user the program has loaded the player's data
                AppInformer.InformUser("Bot Loaded: " + FFACE.Player.Name);

                // Set the main window's title to the player's name. 
                MainWindowTitle = "EasyFarm - " + FFACE.Player.Name;
            }            
        }

        /// <summary>
        /// Shutsdown the program. 
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
        /// Shows the main window when we click this icon in the system tray. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TrayIcon_Click(object sender, System.EventArgs e)
        {
            MasterView.View.WindowState = WindowState.Normal;
            MasterView.View.ShowInTaskbar = true;
            m_trayIcon.Visible = false;
        }

        /// <summary>
        /// Minimizes the application to the system tray. 
        /// </summary>
        /// <param name="e"></param>
        public void OnStateChanged(object sender, System.EventArgs e)
        {
            // Perform tray icon information update here to 
            // receive current title bar information. 
            m_trayIcon.Text = MainWindowTitle;
            m_trayIcon.BalloonTipText = "EasyFarm has been minimized. ";
            m_trayIcon.BalloonTipTitle = MainWindowTitle;

            if (MasterView.View.mnuMinimizeToTray.IsChecked && MasterView.View.WindowState == WindowState.Minimized)
            {
                this.m_trayIcon.Visible = true;
                this.m_trayIcon.ShowBalloonTip(30);
                MasterView.View.ShowInTaskbar = false;
            }
        }
    }
}