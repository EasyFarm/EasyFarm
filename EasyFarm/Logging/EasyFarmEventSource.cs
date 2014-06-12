using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Logging
{
    [EventSource(Name = "EasyFarm")]
    public class EasyFarmEventSource : EventSource
    {
        private static readonly Lazy<EasyFarmEventSource> Instance =
            new Lazy<EasyFarmEventSource>(() => new EasyFarmEventSource());

        public static EasyFarmEventSource Log
        {
            get { return Instance.Value; }
        }

        [Event(100, Message = "Application Started")]
        public void ApplicationStart(string startMessage, string userName)
        {
            if (this.IsEnabled())
            {
                WriteEvent(100, startMessage, String.Format
                    ("User: {0} has entered the building...", userName));
            }
        }
    }
}
