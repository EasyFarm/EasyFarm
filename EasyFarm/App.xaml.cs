
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

using EasyFarm.State;

using EasyFarm.ViewModels;
using EasyFarm.Views;
using FFACETools;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Reflection;
using System.Windows;
using ZeroLimits.FarmingTool;

namespace EasyFarm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static FFACE _fface;
        private static FarmingTools _farmingTools;
        
        public static FarmingTools FarmingTools
        {
            get { return _farmingTools?? (_farmingTools = FarmingTools.GetInstance(_fface)); }
            set { _farmingTools= value; }
        }

        public static IEventAggregator EventAggregator;

        public static GameEngine GameEngine { get; set;}

        public App() 
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;            
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Set up the event aggregator for updates to the status bar from 
            // multiple view models.
            EventAggregator = new EventAggregator();

            // Let user select ffxi process
            frmStartup ProcessSelectionScreen = new frmStartup();
            ProcessSelectionScreen.ShowDialog();

            // Validate the selection
            var m_process = ProcessSelectionScreen.POL_Process;


            if (m_process == null)
            {
                System.Windows.Forms.MessageBox.Show("No valid process was selected: Exiting now.");
                Environment.Exit(0);
            }

            _fface = ProcessSelectionScreen.FFXI_Session;

            GameEngine = new GameEngine(_fface);

            // Set up the game engine if valid.
            FarmingTools.LoadSettings();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            FarmingTools.SaveSettings();
        }

        /// <summary>
        /// Update the user on what's happening.
        /// </summary>
        /// <param name="message">The message to display in the statusbar</param>
        public static void InformUser(String message, params object[] values)
        {
            EventAggregator.GetEvent<StatusBarUpdateEvent>().Publish(String.Format(message, values));
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
