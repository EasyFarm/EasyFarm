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
using EasyFarm.Logging;
using EasyFarm.Properties;
using EasyFarm.Parsing;
using EasyFarm.ViewModels;
using SimpleInjector;

namespace EasyFarm
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <summary>
        ///     XML parser for looking up ability, spell and weaponskill data.
        /// </summary>
        public static AbilityService AbilityService;

        private Container _container = new Container();

        public App()
        {
            Application.Current.DispatcherUnhandledException += (sender, e) =>
            {
                Logger.Log(new LogEntry(LoggingEventType.Fatal, "Unhandled Exception", e.Exception));
                MessageBox.Show(e.Exception.Message, "An exception has occurred. ", MessageBoxButton.OK, MessageBoxImage.Error);
            };
        }

        /// <summary>
        ///     Gets the user's selected fface Session and
        ///     starts up the program.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LogViewModel.Write("Resources loaded");
            AbilityService = new AbilityService("resources");
            LogViewModel.Write("Application starting");
            Logger.Log(new LogEntry(LoggingEventType.Information, "EasyFarm Started ..."));
        }

        /// <summary>
        ///     Save the settings to file via an XML Serializer.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            LogViewModel.Write("Application exiting");
            Settings.Default.Save();
        }
    }
}