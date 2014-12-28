using EasyFarm.States;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EasyFarm.ViewModels
{
    /// <summary>
    /// A class to locate all enabled view models. 
    /// </summary>
    public class Locator<T, TResult> where T : Attribute
    {
        /// <summary>
        /// Returns the list of all view models marked as enabled by
        /// ViewModelAttributes. 
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
                    viewModel.VMName = vmattribute.Name;
                    return viewModel;
                }));

            return viewModels.ToList();
        }

        public List<BaseState> GetEnabledStates(FFACE fface)
        {
            var states = GetMarkedTypes()
                .Where(x => x.GetCustomAttributes<StateAttribute>(false)
                .Any(attr => attr.Enabled))
                .SelectMany(state => state.GetCustomAttributes<StateAttribute>()
                .Select(x =>
                {
                    var ctor = state.GetConstructor(new[] { typeof(FFACE) });
                    if (ctor != null)
                    {
                        var instance = (BaseState)ctor.Invoke(new object[] { fface });
                        instance.Enabled = x.Enabled;
                        instance.Priority = x.Priority;
                        return instance;
                    }
                    return null;
                }));
            return states.ToList();
        }

        /// <summary>
        /// Create an object with a given contructors parameters from a given type.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="constructorArgs"></param>
        /// <returns></returns>
        public Object ConstructItem(Type x, Type[] constructorArgs)
        {
            var ci = x.GetConstructor(constructorArgs);
            if (ci == null) return default(TResult);
            var value = x.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            return value == null ? default(TResult) : ci.Invoke(constructorArgs);
        }

        /// <summary>
        /// Returns the list of all view models marked as enabled by
        /// ViewModelAttributes. 
        /// </summary>
        /// <returns></returns>
        public List<Object> GetMarkedClasses(Type[] constructorArgs)
        {
            return ConstructMarkedClasses(GetMarkedTypes(), constructorArgs);
        }

        /// <summary>
        /// Create a list of objects with the given constructor paramters from a list of types. 
        /// </summary>
        /// <param name="types"></param>
        /// <param name="constructorArgs"></param>
        /// <returns></returns>
        public List<Object> ConstructMarkedClasses(List<Type> types, Type[] constructorArgs)
        {
            return types.Select(x => ConstructItem(x, constructorArgs)).ToList();
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
