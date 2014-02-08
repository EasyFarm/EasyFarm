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
