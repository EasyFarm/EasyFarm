// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013 Mykezero
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
using System.Windows;
using EasyFarm.Infrastructure;
using EasyFarm.Logging;
using EasyFarm.ViewModels;

namespace EasyFarm
{
    public partial class App
    {
        private static AppBoot _appBoot;
        private static Action<App> DefaultInitializer { get; set; } = InitializeApp;

        protected override void OnStartup(StartupEventArgs e)
        {
            Current.DispatcherUnhandledException += (sender, ev) => { UnhandledException(ev.Exception); };
            DefaultInitializer.Invoke(this);
        }

        private static void InitializeApp(App app)
        {
            _appBoot = new AppBoot(app);
            _appBoot.Initialize();
            _appBoot.Navigate<MasterViewModel>();
            _appBoot.Show();
        }

        /// <summary>
        ///     Save the settings to file via an XML Serializer.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            _appBoot.Exit();
        }

        private static void UnhandledException(Exception e)
        {
            Logger.Log(new LogEntry(LoggingEventType.Fatal, "Unhandled Exception", e));
            MessageBox.Show(e.Message, "An exception has occurred. ", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}