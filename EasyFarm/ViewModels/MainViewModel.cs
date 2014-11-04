
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

using EasyFarm.Views;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Windows.Input;
using ZeroLimits.FarmingTool;
using System.Linq;
using EasyFarm.UserSettings;

namespace EasyFarm.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            // Get events from view models to update the status bar's text.
            EventAggregator.GetEvent<StatusBarUpdateEvent>().Subscribe((a) => { StatusBarText = a; });

            // Tell the user the program has loaded the player's data
            InformUser("Bot Loaded: " + FFACE.Player.Name);

            // Create start command handler.
            StartCommand = new DelegateCommand(Start);

            ExitCommand = new DelegateCommand(Exit);

            SaveCommand = new DelegateCommand(Save);
        }

        public String StatusBarText
        {
            get { return Config.Instance.StatusBarText; }
            set { this.SetProperty(ref Config.Instance.StatusBarText, value); }
        }

        public bool IsWorking 
        {
            get { return GameEngine.IsWorking; }
            set { SetProperty(ref GameEngine.IsWorking, value); }
        }
       
        public ICommand StartCommand { get; set; }

        public ICommand ExitCommand { get; set; }

        public DelegateCommand SaveCommand { get; set; }

        public void Start()
        {
            if (GameEngine.IsWorking)
            {
                InformUser("Program paused.");
                GameEngine.Stop();
            }
            else
            {
                InformUser("Program running.");
                GameEngine.Start();
            }
        }

        private void Save()
        {
            Config.Instance.SaveSettings();
        }

        private void Exit()
        {
            App.Current.Shutdown();
        }
    }
}
