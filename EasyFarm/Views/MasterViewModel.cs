
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

using System.Windows;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Windows.Input;
using System.Linq;
using EasyFarm.UserSettings;
using System.Collections.ObjectModel;
using EasyFarm.Logging;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using EasyFarm.Views;
using System.Xml.Serialization;
using Microsoft.Win32;
using EasyFarm.Classes;
using NotifyIcon = System.Windows.Forms.NotifyIcon;
using System.IO;

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
        /// Whether main content or settings window is shown. 
        /// </summary>
        public bool IsSettingsShown { get; set; }

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

            // Toggles start / pause button text. 
            IsSettingsShown = false;

            // Get events from view models to update the status bar's text.
            EventAggregator.GetEvent<StatusBarUpdateEvent>().Subscribe(a => { StatusBarText = a; });

            // Bind commands to their handlers. 
            StartCommand = new DelegateCommand(Start);
            ExitCommand = new DelegateCommand(Exit);
            SaveCommand = new DelegateCommand(Save);
            LoadCommand = new DelegateCommand(Load);
            SettingsCommand = new DelegateCommand(ShowSettings);
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
        /// Toggles whether the settings or main window is shown. 
        /// </summary>
        private void ShowSettings()
        {
            IRegionManager regionManager =
                ServiceLocator.Current.GetInstance<IRegionManager>();

            var region = regionManager.Regions["MainRegion"];

            if (!IsSettingsShown)
            {
                SettingsHeader = "_Main...";
                region.Activate(region.Views.OfType<SettingsView>().First());
                IsSettingsShown = true;
            }
            else
            {
                SettingsHeader = "_Settings...";
                region.Activate(region.Views.OfType<MainView>().First());
                IsSettingsShown = false;
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
            get { return GameEngine.IsWorking; }
            set { SetProperty(ref GameEngine.IsWorking, value); }
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
        /// Internal backing for the settings button text. 
        /// </summary>
        private string _settingsHeader = "_Settings...";

        /// <summary>
        /// Stores the text for the settings button. 
        /// </summary>
        public string SettingsHeader
        {
            get { return _settingsHeader; }
            set { SetProperty(ref _settingsHeader, value); }
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
        /// Load the settings window. 
        /// </summary>
        public DelegateCommand SettingsCommand { get; set; }

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
                InformUser("No process has been selected.");
                return;
            }

            if (GameEngine.IsWorking)
            {
                Logger.Write.BotStop("Bot now paused");
                InformUser("Program paused.");
                GameEngine.Stop();
                StartPauseHeader = "St_art";
            }
            else
            {
                Logger.Write.BotStart("Bot now running");
                InformUser("Program running.");
                GameEngine.Start();
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
                ViewModelBase.InformUser("Settings have been saved.");
                Logger.Write.SaveSettings("Settings saved");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                ViewModelBase.InformUser("Failed to save settings.");
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
                    ViewModelBase.InformUser("Failed to load settings.");
                    return;
                }

                // Inform the user of our success. 
                Config.Instance = settings;
                ViewModelBase.InformUser("Settings have been loaded.");
                Logger.Write.SaveSettings("Settings loaded");
            }
            catch (InvalidOperationException)
            {
                ViewModelBase.InformUser("Failed to load settings.");
            }
        }

        /// <summary>
        /// Selects a process to user for this application. 
        /// </summary>
        private void SelectProcess()
        {
            // Let user select ffxi process
            frmStartup ProcessSelectionScreen = new frmStartup();
            ProcessSelectionScreen.ShowDialog();

            // Validate the selection
            var m_process = ProcessSelectionScreen.SelectedProcess;

            // Check if the user made a selection. 
            if (m_process == null)
            {
                Logger.Write.ProcessNotFound("Process not found");
                ViewModelBase.InformUser("No valid process was selected.");
                return;
            }

            Logger.Write.ProcessFound("Process found");

            // Save the selected fface instance. 
            var FFACE = ProcessSelectionScreen.SelectedSession;

            // Free up and stop timer from working. 
            ProcessSelectionScreen.ProcessWatcher.Dispose();

            ViewModelBase.SetSession(FFACE);

            // Tell the user the program has loaded the player's data
            InformUser("Bot Loaded: " + FFACE.Player.Name);

            // Set the main window's title to the player's name. 
            MainWindowTitle = "EasyFarm - " + FFACE.Player.Name;
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

namespace EasyFarm.UserSettings
{
    public partial class Config
    {
        /// <summary>
        /// The text dislayed at the bottom of the screen
        /// </summary>
        [XmlIgnore]
        public String StatusBarText;

        /// <summary>
        /// The window's name: player's name. 
        /// </summary>
        [XmlIgnore]
        public string MainWindowTitle;
    }
}