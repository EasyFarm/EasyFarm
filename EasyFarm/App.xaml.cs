
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

namespace EasyFarm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// The files we currently use for AbilityService's resource file parsing. 
        /// </summary>
        public static readonly String[] resources = { "spells.xml", "abils.xml" };

        /// <summary>
        /// The url where the resources may be downloaded. 
        /// </summary>
        public const String resourcesUrl =
            "http://www.ffevo.net/topic/2923-ashita-and-ffacetools-missing-resource-files/";

        /// <summary>
        /// Set up the assembly resolution code to find embedded dll files. 
        /// Reduces the amount of dll files in the executable's working directory. 
        /// </summary>
        public App()
        {
            var EventListener = new ObservableEventListener();
            EventListener.EnableEvents(Logger.Write, EventLevel.Verbose);
            Logger.Write.ApplicationStart("Application starting");
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
        }

        ~App()
        {
            Logger.Write.ApplicationStart("Application exiting");
        }

        /// <summary>
        /// Gets the user's selected FFACE Session and 
        /// starts up the program. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Check if the resource files exist.
            if (!Directory.Exists("resources"))
            {
                Logger.Write.ResourceFolderMissing("Resource folder missing");

                MessageBox.Show(
                    String.Format("You're missing the resources folder. You can download it from : {0}", resourcesUrl
                    ));

                Environment.Exit(0);
            }

            if (resources.Any(x => !File.Exists(Path.Combine("resources", x))))
            {
                String[] missing = resources.Where(x => !File.Exists(Path.Combine("resources", x))).ToArray();
                String message = String.Join(" , ", missing);

                Logger.Write.ResourceFileMissing("Resources missing " + String.Join(" , ", missing));

                MessageBox.Show(
                    String.Format("You're missing {0} resource files. You can download them from : {1}.",
                    message, resourcesUrl));
                Environment.Exit(0);
            }

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

            ViewModelBase.SetSession(FFACE);

            // new DebugSpellCasting(_fface).Show();
            // new DebugCreatures(_fface, FarmingTools.UnitService).Show();
            // var dbc = new DebugCreatures(FarmingTools.FFACE, FarmingTools.UnitService);
            // dbc.Show();
        }

        /// <summary>
        /// Save the settings to file via an XML Serializer. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            Config.Instance.SaveSettings();
        }

        // Thanks to atom0s for assembly embedding code!

        /// <summary>
        /// Assembly resolve event callback to load embedded libraries.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var dllName = args.Name.Contains(",") ? args.Name.Substring(0, args.Name.IndexOf(",", System.StringComparison.InvariantCultureIgnoreCase)) : args.Name.Replace(".dll", "");
            if (dllName.ToLower().EndsWith(".resources"))
                return null;

            var fullName = string.Format("EasyFarm.Embedded.{0}.dll", new AssemblyName(args.Name).Name);
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fullName))
            {
                if (stream == null)
                    return null;

                var data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                return Assembly.Load(data);
            }
        }
    }
}
