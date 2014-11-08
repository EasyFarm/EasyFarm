
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


using EasyFarm.State;
using EasyFarm.UserSettings;
using FFACETools;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using ZeroLimits.FarmingTool;

namespace EasyFarm.ViewModels
{
    public class ViewModelBase : BindableBase
    {
        public static FFACE FFACE { get; set; }

        public static IEventAggregator EventAggregator { get; set; }

        public static FTools FTools { get; set; }

        public static GameEngine GameEngine { get; set; }

        static ViewModelBase()
        {
            // Set up the event aggregator for updates to the status bar from 
            // multiple view models.
            EventAggregator = new EventAggregator();
        }

        /// <summary>
        /// Update the user on what's happening.
        /// </summary>
        /// <param name="message">The message to display in the statusbar</param>
        public static void InformUser(String message, params object[] values)
        {
            EventAggregator.GetEvent<StatusBarUpdateEvent>().Publish(String.Format(message, values));
        }

        public static void SetSession(FFACE fface)
        {
            // Save FFACE Instance
            FFACE = fface;

            // Create FarmngTools object. 
            FTools = new FTools(FFACE);

            // Create a new game engine to control our character. 
            GameEngine = new GameEngine(FFACE);

            // Set FFACE session for config. 
            Config.Instance.FFACE = FFACE;

            // Loads up all settings. 
            Config.Instance.LoadSettings();
        }
    }
}
