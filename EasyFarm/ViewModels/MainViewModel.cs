
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

namespace EasyFarm.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Internal list of view models. 
        /// </summary>
        private ObservableCollection<ViewModelBase> _viewModels;

        /// <summary>
        /// List of dynamically found view models. 
        /// </summary>
        public ObservableCollection<ViewModelBase> ViewModels
        {
            get { return _viewModels; }
            set 
            {
                SetProperty(ref _viewModels, value);
            }
        }

        /// <summary>
        /// Interal stating index for the currently focused tab.
        /// </summary>
        private int _selectedIndex;

        /// <summary>
        /// Index for the currently focused tab. 
        /// </summary>
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }

        public MainViewModel()
        {
            var locator = new Locator<ViewModelAttribute, ViewModelBase>();

            // Get all enabled view models. 
            ViewModels = new ObservableCollection<ViewModelBase>(
                locator.GetEnabledViewModels()
                .Where(x => x != null)
                .OrderBy(x => x.VMName));

            // Get events from view models to update the status bar's text.
            EventAggregator.GetEvent<StatusBarUpdateEvent>().Subscribe(a => { StatusBarText = a; });

            // Tell the user the program has loaded the player's data
            InformUser("Bot Loaded: " + FFACE.Player.Name);

            // Set the main window's title to the player's name. 
            MainWindowTitle = "EasyFarm - " + FFACE.Player.Name;

            // Create start command handler.
            StartCommand = new DelegateCommand(Start);

            ExitCommand = new DelegateCommand(Exit);

            SaveCommand = new DelegateCommand(Save);
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

        private string _startStopHeader = "St_art";
        /// <summary>
        /// Binding for File -> Start/Pause text.
        /// </summary>
        public string StartPauseHeader
        {
            get
            {
                return _startStopHeader;
            }
            set
            {
                SetProperty(ref _startStopHeader, value);
            }
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
        /// Tells the program to start farming. 
        /// </summary>
        public void Start()
        {
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
            Logger.Write.SaveSettings("Settings saved");
            Config.Instance.SaveSettings();
        }

        /// <summary>
        /// Shutsdown the program. 
        /// </summary>
        private void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}
