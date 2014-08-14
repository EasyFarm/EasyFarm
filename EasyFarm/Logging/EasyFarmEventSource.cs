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
        public class Keyworlds
        {
            public const EventKeywords Diagnostic = (EventKeywords)1;
            public const EventKeywords Performance = (EventKeywords)2;
            public const EventKeywords States = (EventKeywords)4;
        }

        private static readonly Lazy<EasyFarmEventSource> Instance =
            new Lazy<EasyFarmEventSource>(() => new EasyFarmEventSource());

        public EasyFarmEventSource() { }

        public static EasyFarmEventSource Log
        {
            get { return Instance.Value; }
        }

        /////////////////////////////////////////////////////////////
        // Application Events
        /////////////////////////////////////////////////////////////

        /*Application Events: 1:99*/
        [Event(1, Message = "Application Started",
        Level = EventLevel.Informational, Keywords = Keyworlds.Performance)]
        public void ApplicationStart(string message)
        {
            if (this.IsEnabled()) WriteEvent(1, message);
        }

        [Event(2, Message = "Application Exited",
        Level = EventLevel.Informational, Keywords = Keyworlds.Performance)]
        public void ApplicationExit(string message)
        {
            if (this.IsEnabled()) WriteEvent(2, message);
        }

        /////////////////////////////////////////////////////////////
        // State Events
        /////////////////////////////////////////////////////////////

        /*State Events: 100-199*/
        [Event(100, Message = "{0} State Check", 
        Level=EventLevel.Informational, Keywords=Keyworlds.States)]
        public void StateCheck(string message, string name, bool check)
        {
            if (this.IsEnabled()) WriteEvent(2, name, check);
        }

        [Event(102, Message = "{0} State Enter",
        Level=EventLevel.Informational, Keywords=Keyworlds.States)]
        public void StateEnter(string name)
        {
            if (this.IsEnabled()) WriteEvent(2, name);
        }

        [Event(102, Message = "{0} State Running",
        Level = EventLevel.Informational, Keywords = Keyworlds.States)]
        public void StateRun(string message)
        {
            if (this.IsEnabled()) WriteEvent(2, message);
        }
    }
}
