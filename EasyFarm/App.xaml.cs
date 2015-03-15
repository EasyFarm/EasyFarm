
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

namespace EasyFarm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly AbilityService AbilityService;

        static App()
        {
            // Create the ability service passing to it the resources 
            // folder named "resources"
            AbilityService = new AbilityService("resources");
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

            Logger.Write.ResourcesLocated("Resources located");

            // Let user select ffxi process
            frmStartup ProcessSelectionScreen = new frmStartup();
            ProcessSelectionScreen.ShowDialog();

            // Validate the selection
            var m_process = ProcessSelectionScreen.SelectedProcess;

            // Check if the user made a selection. 
            if (m_process == null)
            {
                Logger.Write.ProcessNotFound("Process not found");
                MessageBox.Show("No valid process was selected: Exiting now.");
                Environment.Exit(0);
            }

            Logger.Write.ProcessFound("Process found");

            // Save the selected fface instance. 
            var FFACE = ProcessSelectionScreen.SelectedSession;

            // Free up and stop timer from working. 
            ProcessSelectionScreen.ProcessWatcher.Dispose();

            ViewModelBase.SetSession(FFACE);

            // new DebugSpellCasting(_fface).Show();
            // new DebugCreatures(_fface, FarmingTools.UnitService).Show();
            // var dbc = new DebugCreatures(FarmingTools.FFACE, FarmingTools.UnitService);
            // dbc.Show();
                
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
        }        
    }
}
