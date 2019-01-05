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
using System.Windows;
using EasyFarm.Classes;
using EasyFarm.Handlers;
using EasyFarm.Persistence;
using MahApps.Metro.Controls.Dialogs;
using Ninject.Modules;
using Ninject.Extensions.Conventions;

namespace EasyFarm.Infrastructure
{
    public class ApplicationBindingsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<App>().ToMethod(x => Application.Current as App);
            Bind<TabViewModels>().ToSelf();
            Bind<LibraryUpdater>().ToSelf();
            Bind<SelectCharacterRequestHandler>().ToSelf();
            Bind<SelectAbilityRequestHandler>().ToSelf();
            Bind<IDialogCoordinator>().To<DialogCoordinator>();
            Bind<IPersister>().To<Persister>();

            this.Bind(x => x.FromThisAssembly().SelectAllClasses().EndingWith("ViewModel").BindToSelf());
        }
    }
}
