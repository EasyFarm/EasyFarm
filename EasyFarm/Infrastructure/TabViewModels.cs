using System;
using System.Collections.Generic;
using System.Linq;
using DryIoc;
using EasyFarm.ViewModels;

namespace EasyFarm.Infrastructure
{
    public class TabViewModels
    {
        public IList<IViewModel> ViewModels = new List<IViewModel>();

        public TabViewModels(Container container)
        {
            foreach (KeyValuePair<Type, int> availableTab in AvailableTabs)
            {
                ViewModels.Add((IViewModel)container.Resolve(availableTab.Key));
            }

            ViewModels = ViewModels
                .Where(x => AvailableTabs.ContainsKey(x.GetType()))
                .OrderBy(x => AvailableTabs[x.GetType()])
                .ToList();
        }

        /// <summary>
        /// The available tabs in the main view with their given display order.
        /// </summary>
        public Dictionary<Type, int> AvailableTabs => new Dictionary<Type, int>
        {
            { typeof(BattlesViewModel), 1 },
            { typeof(TargetingViewModel), 2 },
            { typeof(RestingViewModel), 3 },
            { typeof(RoutesViewModel), 4 },
            { typeof(FollowViewModel), 5 },
            { typeof(LogViewModel), 6 },
            { typeof(SettingsViewModel), 7 }
        };
    }
}