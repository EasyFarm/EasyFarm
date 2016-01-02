using System;
using System.Windows;
using Prism.Regions;

namespace EasyFarm.Infrastructure
{
    public class ViewNavigator
    {
        public static IRegionManager RegionManager { get; set; }

        public static void Navigate<T>() where T : DependencyObject
        {
            var viewName = typeof(T).Name;

            if (!RegionManager.Regions.ContainsRegionWithName(viewName))
            {
                RegionManager.RegisterViewWithRegion(Regions.MainRegion, typeof(T));
            }

            RegionManager.RequestNavigate(Regions.MainRegion, new Uri(viewName, UriKind.Relative));
        }
    }
}
