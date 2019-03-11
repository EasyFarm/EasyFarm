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
using System.Collections.Generic;
using System.Windows;
using EasyFarm.Classes;
using EasyFarm.Handlers;
using EasyFarm.Persistence;
using EasyFarm.ViewModels;
using EasyFarm.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace EasyFarm.Infrastructure
{
    public class CompositeRoot
    {
        private static readonly EventMessenger EventMessenger = new EventMessenger();

        /// <summary>
        /// Creates an instance of the given type.
        /// </summary>
        /// <param name="requestedType"></param>
        /// <returns></returns>
        /// <remarks>
        /// Every object in the composite root should be instantiates before returning the requested type.
        /// Otherwise, classes may not work correctly with each other, if they use an indirect way of communicating
        /// such as through events.
        /// </remarks>
        private object CreateInstance(Type requestedType)
        {
            MetroWindow metroWindow = Application.Current.MainWindow as MetroWindow;
            App app = Application.Current as App;
            LibraryUpdater libraryUpdater = new LibraryUpdater();
            DialogCoordinator dialogCoordinator = new DialogCoordinator();
            Persister persister = new Persister();
            UpdateEliteAPI updateEliteAPI = new UpdateEliteAPI(libraryUpdater, dialogCoordinator, EventMessenger);
            SelectCharacter selectCharacterRequest = new SelectCharacter(EventMessenger, metroWindow);
            SelectAbilityRequestHandler selectAbilityRequestHandler = new SelectAbilityRequestHandler(metroWindow);
            BattlesViewModel battlesViewModel = new BattlesViewModel();
            FollowViewModel followViewModel = new FollowViewModel();
            IgnoredViewModel ignoredViewModel = new IgnoredViewModel();
            LogViewModel logViewModel = new LogViewModel();            
            SelectProcessViewModel selectProcessViewModel = new SelectProcessViewModel(new SelectProcessDialog());
            RestingViewModel restingViewModel = new RestingViewModel();
            RoutesViewModel routesViewModel = new RoutesViewModel();
            SettingsViewModel settingsViewModel = new SettingsViewModel();
            TargetingViewModel targetingViewModel = new TargetingViewModel();
            TargetsViewModel targetsViewModel = new TargetsViewModel();            
            TabViewModels tabViewModels = new TabViewModels(new List<IViewModel>()
            {
                battlesViewModel,
                targetingViewModel,
                restingViewModel,
                routesViewModel,
                followViewModel,
                logViewModel,
                settingsViewModel,
            });
            MainViewModel mainViewModel = new MainViewModel(tabViewModels);            
            MasterViewModel masterViewModel = new MasterViewModel(mainViewModel, EventMessenger);

            if (requestedType == typeof(EventMessenger)) return EventMessenger;
            if (requestedType == typeof(App)) return app;
            if (requestedType == typeof(LibraryUpdater)) return libraryUpdater;
            if (requestedType == typeof(SelectCharacter)) return selectCharacterRequest;
            if (requestedType == typeof(SelectAbilityRequestHandler)) return selectAbilityRequestHandler;
            if (requestedType == typeof(IDialogCoordinator)) return dialogCoordinator;
            if (requestedType == typeof(IPersister)) return persister;
            if (requestedType == typeof(UpdateEliteAPI)) return updateEliteAPI;
            if (requestedType == typeof(BattlesViewModel)) return battlesViewModel;
            if (requestedType == typeof(FollowViewModel)) return followViewModel;
            if (requestedType == typeof(IgnoredViewModel)) return ignoredViewModel;
            if (requestedType == typeof(LogViewModel)) return logViewModel;
            if (requestedType == typeof(TabViewModels)) return tabViewModels;
            if (requestedType == typeof(MainViewModel)) return mainViewModel;
            if (requestedType == typeof(MasterViewModel)) return masterViewModel;
            if (requestedType == typeof(SelectProcessViewModel)) return selectProcessViewModel;
            if (requestedType == typeof(RestingViewModel)) return restingViewModel;
            if (requestedType == typeof(RoutesViewModel)) return routesViewModel;
            if (requestedType == typeof(SettingsViewModel)) return settingsViewModel;
            if (requestedType == typeof(TargetingViewModel)) return targetingViewModel;
            if (requestedType == typeof(TargetsViewModel)) return targetsViewModel;

            throw new InvalidOperationException($"Could not create instance of requested type {requestedType.Name}");
        }

        public T Get<T>()
        {
            return (T)CreateInstance(typeof(T));
        }

        public object Get(Type requestedType)
        {
            return CreateInstance(requestedType);
        }
    }
}