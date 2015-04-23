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

using EasyFarm.ViewModels;
using Microsoft.Practices.Prism.PubSubEvents;

namespace EasyFarm.Classes
{
    /// <summary>
    ///     Updates the main window's status bar text to
    ///     inform the user of important information.
    /// </summary>
    public class AppInformer
    {
        static AppInformer()
        {
            // Set up the event aggregator for updates to the status bar from 
            // multiple view models.
            EventAggregator = new EventAggregator();
        }

        /// <summary>
        ///     Sends messages mostly to the status bar.
        /// </summary>
        public static IEventAggregator EventAggregator { get; set; }

        /// <summary>
        ///     Update the user on what's happening.
        /// </summary>
        /// <param name="message">The message to display in the statusbar</param>
        /// <param name="values"></param>
        public static void InformUser(string message, params object[] values)
        {
            EventAggregator.GetEvent<StatusBarUpdateEvent>().Publish(string.Format(message, values));
        }
    }
}