using EasyFarm.ViewModels;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Classes
{
    public class AppInformer
    {
        /// <summary>
        /// Sends messages mostly to the status bar. 
        /// </summary>
        public static IEventAggregator EventAggregator { get; set; }

        static AppInformer()
        {
            // Set up the event aggregator for updates to the status bar from 
            // multiple view models.
            EventAggregator = new EventAggregator();            
        }

        /// <summary>
        /// Update the user on what's happening.
        /// </summary>
        /// <param name="message">The message to display in the statusbar</param>
        /// <param name="values"></param>
        public static void InformUser(String message, params object[] values)
        {
            EventAggregator.GetEvent<StatusBarUpdateEvent>().Publish(String.Format(message, values));
        }
    }
}
