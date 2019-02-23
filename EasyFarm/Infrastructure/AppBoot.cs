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
using EasyFarm.Classes;
using EasyFarm.Logging;
using EasyFarm.Persistence;
using EasyFarm.UserSettings;
using EasyFarm.ViewModels;
using EasyFarm.Views;

namespace EasyFarm.Infrastructure
{
    public class AppBoot
    {
        private readonly CompositeRoot _container;
        private readonly App _app;

        public AppBoot(App app)
        {
            _app = app;
            _container = new CompositeRoot();
            _app.MainWindow = new MasterView();
        }

        public void Initialize()
        {
            SystemTray.ConfigureTray(_app.MainWindow);
            LogViewModel.Write("Resources loaded");
            LogViewModel.Write("Application starting");
            Logger.Log(new LogEntry(LoggingEventType.Information, "EasyFarm Started ..."));
        }

        public void Navigate<TViewModel>()
        {
            _app.MainWindow.DataContext = _container.Get(typeof(TViewModel));
        }

        public void Show()
        {
            _app.MainWindow?.Show();
        }

        public void Exit()
        {
            IPersister persister = _container.Get<IPersister>();
            string characterName = ViewModelBase.FFACE?.Player?.Name;
            string fileName = $"{characterName}.eup";

            if (!String.IsNullOrWhiteSpace(fileName))
            {
                persister.Serialize(fileName, Config.Instance);
            }

            LogViewModel.Write("Application exiting");
        }
    }
}