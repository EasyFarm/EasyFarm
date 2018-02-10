// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
// 
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
using System;
using EasyFarm.Infrastructure;
using GalaSoft.MvvmLight.Messaging;

namespace EasyFarm.Classes
{
    /// <summary>
    ///     Updates the main window's status bar text to
    ///     inform the user of important information.
    /// </summary>
    public class AppServices
    {
        /// <summary>
        ///     Sends messages mostly to the status bar.
        /// </summary>
        public static IMessenger EventAggregator { get; set; } = Messenger.Default;

        /// <summary>
        ///     Update the user on what's happening.
        /// </summary>
        /// <param name="message">The message to display in the statusbar</param>
        /// <param name="values"></param>
        public static void InformUser(string message, params object[] values)
        {
            var statusBarEvent = new Events.StatusBarEvent {Message = string.Format(message, values)};
            PublishEvent(statusBarEvent);
        }

        public static void UpdateTitle(string message, params object[] values)
        {
            var statusBarEvent = new Events.TitleEvent { Message = string.Format(message, values) };
            PublishEvent(statusBarEvent);
        }

        public static void SendPauseEvent()
        {
            PublishEvent<Events.PauseEvent>();
        }

        public static void SendResumeEvent()
        {
            PublishEvent<Events.ResumeEvent>();
        }

        private static void PublishEvent<T>(T value = default(T)) where T : class
        {
            EventAggregator.Send(value);
        }

        public static void RegisterEvent<T>(object receipent, Action<T> action)
        {
            EventAggregator.Register<T>(receipent, action);
        }
    }
}