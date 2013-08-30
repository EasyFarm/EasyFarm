using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive;
using System.Windows;
using System.Reactive.Linq;

namespace EasyFarm.ExtensionMethods
{
    public static class RXExtensions
    {
        /// <summary>
        /// Returns the data for a controls property. 
        /// This data is distinct and is pumped every second.
        /// </summary>
        /// <param name="Control">Control to receive data from</param>
        /// <param name="Property">The property of the control from which to get data</param>
        /// <returns></returns>
        public static IObservable<EventPattern<EventArgs>> GetControlData(this UIElement Control, String Property)
        {
            return Observable.FromEventPattern<EventArgs>(Control, Property).DistinctUntilChanged().Throttle(TimeSpan.FromSeconds(1));
        }
    }
}
