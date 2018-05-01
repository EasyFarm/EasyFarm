using System;
using EasyFarm.UserSettings;
using MemoryAPI;

namespace EasyFarm.Classes
{
    /// <summary>
    /// Makes services available globally by providing build / create methods. 
    /// </summary>
    /// <remarks>
    /// Using this class to move the construction of services out of static methods.
    /// Classes here will eventually be moved somewhere else.
    /// </remarks>
    public class GlobalFactory
    {
        public static IConfigFactory ConfigFactory { get; set; } = new ConfigFactory();
    }
}