/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

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

using EasyFarm.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace EasyFarm.Prism
{
    public class MainModule : IModule
    {
        private readonly IRegionViewRegistry regionViewRegistry;

        public MainModule(IRegionViewRegistry regionViewRegistry)
        {
            this.regionViewRegistry = regionViewRegistry;
        }

        public void Initialize()
        {
            regionViewRegistry.RegisterViewWithRegion("MainRegion", typeof (MainView));
            regionViewRegistry.RegisterViewWithRegion("MainRegion", typeof (SettingsView));
        }
    }
}