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
using EasyFarm.Mvvm;
using EasyFarm.Properties;
using Parsing.Services;
using AutoMapper;
using EasyFarm.Classes;
using MemoryAPI;

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
        public static readonly AbilityService AbilityService;

        static App()
        {
            // Create the ability service passing to it the resources 
            // folder named "resources"
            AbilityService = new AbilityService("resources");
            Logger.Write.ResourcesLocated("Resources loaded");
        }

        public App()
        {
            Application.Current.DispatcherUnhandledException += (sender, e) =>
            {
                MessageBox.Show(e.Exception.Message, "An exception has occurred. ", MessageBoxButton.OK, MessageBoxImage.Error);
            };
        }

        /// <summary>
        ///     Gets the user's selected FFACE Session and
        ///     starts up the program.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {            
            base.OnStartup(e);
            Mapper.CreateMap<IPosition, Position>().ReverseMap();
            Logger.Write.ApplicationStart("Application starting");
            var bootStrapper = new BootStrapper();
            bootStrapper.Run();
        }

        /// <summary>
        ///     Save the settings to file via an XML Serializer.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            Logger.Write.ApplicationStart("Application exiting");
            Settings.Default.Save();
        }
    }
}