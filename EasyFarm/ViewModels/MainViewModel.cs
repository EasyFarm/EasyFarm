
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
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EasyFarm.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public MainViewModel(ref GameEngine Engine, IEventAggregator eventAggregator) :
            base(ref Engine, eventAggregator) 
        {
            // Tell the user the program has loaded the player's data
            InformUser("Bot Loaded: " + Engine.Session.Instance.Player.Name);
            
            // Create start command handler.
            StartCommand = new DelegateCommand(Start);
            
            // Get events from view models to update the status bar's text.
            eventAggregator.GetEvent<StatusBarUpdateEvent>().Subscribe((a) => { StatusBarText = a; });
        }

        public String StatusBarText
        {
            get { return _engine.UserSettings.StatusBarText; }
            set { this.SetProperty(ref _engine.UserSettings.StatusBarText, value); }
        }
       
        public ICommand StartCommand { get; set; }

        public void Start()
        {
            if (_engine.IsWorking)
            {
                InformUser("Program paused.");
                _engine.Stop();
            }
            else
            {
                InformUser("Program running.");
                _engine.Start();
            }
        }
    }
}
