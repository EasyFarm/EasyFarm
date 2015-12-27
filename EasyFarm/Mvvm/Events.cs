using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace EasyFarm.Mvvm
{
    public class Events
    {
        public class StatusBarEvent : PubSubEvent<string> { }

        public class PauseEvent : PubSubEvent<string> { }

        public class ResumeEvent : PubSubEvent<string>{ }
    }
}
