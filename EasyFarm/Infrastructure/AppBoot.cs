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
using EasyFarm.Classes;
using EasyFarm.Handlers;
using EasyFarm.Views;
using Ninject;

namespace EasyFarm.Infrastructure
{
    public class AppBoot
    {
        public IKernel Container { get; set; }
        private readonly App _app;

        public AppBoot(App app)
        {
            _app = app;
            Container = CreateContainer();
            _app.MainWindow = new MasterView();
        }

        public void Initialize()
        {
            SystemTray.ConfigureTray(_app.MainWindow);
        }

        public object ViewModel => _app.MainWindow?.DataContext;

        private IKernel CreateContainer()
        {
            StandardKernel container = new StandardKernel(
                new ApplicationBindingsModule());
            return container;
        }

        public void Navigate<TViewModel>()
        {
            NavigateViewRequestHandler handler = new NavigateViewRequestHandler(_app, Container);
            handler.Handle(typeof(TViewModel)).GetAwaiter();
        }

        public void Show()
        {
            _app.MainWindow?.Show();
        }
    }
}