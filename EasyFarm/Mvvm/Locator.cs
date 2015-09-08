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
    public class Locator<T, TResult> where T : Attribute
    {
        /// <summary>
        /// Returns the list of all view models marked as enabled by ViewModelAttributes.
        /// </summary>
        /// <returns></returns>
        public List<ViewModelBase> GetEnabledViewModels()
        {
            var allMarkedClasses = GetMarkedTypes();

            var enabledViewModels = allMarkedClasses.Where(x =>
                x.GetCustomAttributes<ViewModelAttribute>(false)
                    .Any(attr => attr.Enabled))
                .ToList();

            var viewModels = enabledViewModels
                .SelectMany(vmclass => vmclass.GetCustomAttributes<ViewModelAttribute>(false)
                    .Select(vmattribute =>
                    {
                        var viewModel = (ViewModelBase)ConstructItem(vmclass, new Type[] { });
                        viewModel.VmName = vmattribute.Name;
                        return viewModel;
                    }));

            return viewModels.ToList();
        }

        /// <summary>
        /// Create an object with a given contructors parameters from a given type.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="constructorArgs"></param>
        /// <returns></returns>
        public object ConstructItem(Type x, Type[] constructorArgs)
        {
            var ci = x.GetConstructor(constructorArgs);
            if (ci == null) return default(TResult);
            var value = x.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            return value == null ? default(TResult) : ci.Invoke(constructorArgs);
        }

        /// <summary>
        /// Get a list of types that have been marked by an attribute.
        /// </summary>
        /// <returns></returns>
        public List<Type> GetMarkedTypes()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.GetCustomAttributes(false).Any(attribute => attribute is T))
                .ToList();
        }
    }
}