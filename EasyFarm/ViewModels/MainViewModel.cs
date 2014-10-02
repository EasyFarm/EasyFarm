
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013 - 2014>  <Zerolimits>

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

using EasyFarm.Views;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Windows.Input;
using ZeroLimits.FarmingTool;
using System.Linq;

namespace EasyFarm.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(FarmingTools farmingTools) : base(farmingTools)
        {
            // Tell the user the program has loaded the player's data
            App.InformUser("Bot Loaded: " + farmingTools.FFACE.Player.Name);
        }

        public MainViewModel()
        {
            this.ftools = FarmingTools.GetInstance();

            // Get events from view models to update the status bar's text.
            App.EventAggregator.GetEvent<StatusBarUpdateEvent>().Subscribe((a) => { StatusBarText = a; });

            // Create start command handler.
            StartCommand = new DelegateCommand(Start);

            ExitCommand = new DelegateCommand(Exit);

            SaveCommand = new DelegateCommand(Save);

            SettingsCommand = new DelegateCommand(Settings);
        }

        private void Settings()
        {
            
        }

        public String StatusBarText
        {
            get { return ftools.UserSettings.StatusBarText; }
            set { this.SetProperty(ref ftools.UserSettings.StatusBarText, value); }
        }
       
        public ICommand StartCommand { get; set; }

        public ICommand ExitCommand { get; set; }

        public DelegateCommand SaveCommand { get; set; }

        public DelegateCommand SettingsCommand { get; set; }

        public void Start()
        {
            if (App.GameEngine.IsWorking)
            {
                App.InformUser("Program paused.");
                App.GameEngine.Stop();
            }
            else
            {
                App.InformUser("Program running.");
                App.GameEngine.Start();
            }
        }

        private void Save()
        {
            App.FarmingTools.SaveSettings();
        }

        private void Exit()
        {
            App.Current.Shutdown();
        }

        public string InteractionResultMessage { get; set; }
    }
}
