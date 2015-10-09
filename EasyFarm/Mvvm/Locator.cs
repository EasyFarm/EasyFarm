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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EasyFarm.Mvvm
{
    /// <summary>
    /// A class to locate all enabled view models.
    /// </summary>
    public class Locator
    {
        /// <summary>
        /// Locate and build all view models with their default constructors. 
        /// </summary>
        /// <returns></returns>
        public List<IViewModel> GetEnabledViewModels()
        {
            var typeInfo = from type in Assembly.GetExecutingAssembly().GetTypes()
                           where type.IsClass
                           let attribute = type.GetCustomAttribute<ViewModelAttribute>()
                           where attribute != null && attribute.Enabled
                           select new { type = type, attribute = attribute };

            return typeInfo.Select(info =>
            {
                IViewModel item = (IViewModel)Activator.CreateInstance(info.type);
                item.ViewName = info.attribute.Name;
                return item;
            }).ToList();
        }
    }
}