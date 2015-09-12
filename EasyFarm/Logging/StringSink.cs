using System;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Prism.Mvvm;

namespace EasyFarm.Logging
{
    public sealed class StringSink : BindableBase, IObserver<EventEntry>
    {
        private readonly Action<string> _action;

        public StringSink(Action<string> action)
        {
            _action = action;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(EventEntry value)
        {
            var timestamp = GetTimeStamp(value.Timestamp);
            var message = string.Join(" ", value.Payload);
            _action(timestamp + " " + message);
        }

        public string GetTimeStamp(DateTimeOffset time)
        {
            return string.Join(":", time.Hour, time.Minute, time.Second);
        }
    }
}