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
using System.Threading.Tasks;
using Ninject;

namespace EasyFarm.Handlers
{
    public class NavigateViewRequestHandler
    {
        private readonly App _app;
        private readonly IKernel _container;

        public NavigateViewRequestHandler(App app, IKernel container)
        {
            _app = app;
            _container = container;
        }

        public Task Handle(Type requestedType)
        {
            if (_app.MainWindow == null) return Task.FromResult(true);
            _app.MainWindow.DataContext = _container.Get(requestedType);
            return Task.FromResult(true);
        }
    }
}