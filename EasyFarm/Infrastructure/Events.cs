using Prism.Events;

namespace EasyFarm.Infrastructure
{
    public class Events
    {
        public class StatusBarEvent : PubSubEvent<string>
        {
        }

        public class PauseEvent : PubSubEvent<string>
        {
        }

        public class ResumeEvent : PubSubEvent<string>
        {
        }
    }
}