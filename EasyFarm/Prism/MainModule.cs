using EasyFarm.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Prism
{
    public class MainModule : IModule
    {
        private readonly IRegionViewRegistry regionViewRegistry = null;

        public MainModule(IRegionViewRegistry regionViewRegistry)
        {
            this.regionViewRegistry = regionViewRegistry;
        }

        public void Initialize()
        {
            this.regionViewRegistry.RegisterViewWithRegion("MainRegion", typeof(MainView));
            this.regionViewRegistry.RegisterViewWithRegion("MainRegion", typeof(SettingsView));
        }
    }
}
