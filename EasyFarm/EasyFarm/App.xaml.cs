
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

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;

namespace EasyFarm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        MainWindow Window = null;
        ViewModel Bindings = null;

        public App()
        {
            Bindings = new ViewModel();
            Window = new MainWindow(ref Bindings);            
            Window.DataContext = Bindings;

            Bindings.Engine.LoadSettings();
            Bindings.RefreshConfig();
            Window.Show();

            Exit += (s, e) => {
                Bindings.Engine.SaveSettings(Bindings.Engine);
            };
        }
    }
}
