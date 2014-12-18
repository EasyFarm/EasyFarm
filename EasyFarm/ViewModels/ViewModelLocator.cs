using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.ViewModels
{
    /// <summary>
    /// A class to locate all enabled view models. 
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Returns the list of all view models marked as enabled by
        /// ViewModelAttributes. 
        /// </summary>
        /// <returns></returns>
        public static List<ViewModelBase> GetEnabledViewModels()
        { 
            // Holds the list of enabled view models. 
            List<ViewModelBase> EnabledViewModels = new List<ViewModelBase>();

            // Get all VMs marked with the ViewModelAttribute
            var VMClasses = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => x.IsClass)
                .Where(x => x.GetCustomAttributes<ViewModelAttribute>(false).Any())
                .ToList();

            // For all marked classes...
            foreach (var item in VMClasses)
            {
                // Get the constructors info for creating a new vm. 
                ConstructorInfo ci = item.GetConstructor(new Type[]{});

                if (ci == null) { continue; }

                ViewModelBase vm = null;

                // For each attribute...
                foreach (ViewModelAttribute attribute in item.GetCustomAttributes(typeof(ViewModelAttribute), false))
                {
                    // Create and add view model if enabled. 
                    if (attribute.Enabled)
                    {
                        // Call its constructor and make a new vm
                        vm = (ViewModelBase)ci.Invoke(new Type[] { });
                        
                        // Save into it the attribute's name. 
                        vm.VMName = attribute.Name;

                        // Add the new view model to the list of enabled view models. 
                        EnabledViewModels.Add(vm);
                    }
                }
            }

            // Return all enabled view models. 
            return EnabledViewModels;
        }
    }
}
