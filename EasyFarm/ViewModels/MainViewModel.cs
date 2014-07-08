
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

using EasyFarm.Classes;
using EasyFarm.Classes.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EasyFarm.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(FarmingTools farmingTools) : base(farmingTools)
        {
            // Get events from view models to update the status bar's text.
            App.EventAggregator.GetEvent<StatusBarUpdateEvent>().Subscribe((a) => { StatusBarText = a; });

            // Tell the user the program has loaded the player's data
            App.InformUser("Bot Loaded: " + farmingTools.FFACE.Player.Name);
            
            // Create start command handler.
            StartCommand = new DelegateCommand(Start);
        }

        public String StatusBarText
        {
            get { return farmingTools.UserSettings.StatusBarText; }
            set { this.SetProperty(ref farmingTools.UserSettings.StatusBarText, value); }
        }
       
        public ICommand StartCommand { get; set; }

        public void Start()
        {
            if (farmingTools.GameEngine.IsWorking)
            {
                App.InformUser("Program paused.");
                farmingTools.GameEngine.Stop();
            }
            else
            {
                App.InformUser("Program running.");
                farmingTools.GameEngine.Start();
            }
        }
    }
}
