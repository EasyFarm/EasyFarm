using Prism.Events;

namespace EasyFarm.Infrastructure
{
    public class Events
    {
        public class StatusBarEvent
        {
            public string Message { get; set; }
        }

        public class PauseEvent { }

        public class ResumeEvent { }
    }    
}