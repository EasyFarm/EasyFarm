
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
using EasyFarm.Views;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Linq;
using EasyFarm.UserSettings;
using EasyFarm.Logging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using System.Diagnostics.Tracing;
using System.Collections;
using EasyFarm.Prism;
using Parsing.Services;
using EasyFarm.Components;

namespace EasyFarm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly AbilityService AbilityService;

        public static readonly GameEngine GameEngine;

        static App()
        {
            // Create the ability service passing to it the resources 
            // folder named "resources"
            AbilityService = new AbilityService("resources");
            Logger.Write.ResourcesLocated("Resources loaded");
        }

        /// <summary>
        /// Gets the user's selected FFACE Session and 
        /// starts up the program. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Logger.Write.ApplicationStart("Application starting");                        
                          
            BootStrapper bootStrapper = new BootStrapper();
            bootStrapper.Run();
        }

        /// <summary>
        /// Save the settings to file via an XML Serializer. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            Logger.Write.ApplicationStart("Application exiting");
            EasyFarm.Properties.Settings.Default.Save();
        }        
    }
}
