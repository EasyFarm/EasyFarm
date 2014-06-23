
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
using EasyFarm.UtilityTools;
using EasyFarm.ViewModels;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace EasyFarm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static GameEngine Engine;
        public static IEventAggregator EventAggregator;

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

            // Set up the game engine if valid.
            Engine = GameEngine.GetInstance(ProcessSelectionScreen.POL_Process);

            Engine.LoadSettings();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Engine.SaveSettings(Engine);
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
