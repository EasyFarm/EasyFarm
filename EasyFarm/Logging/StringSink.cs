using System;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.Prism.Mvvm;

namespace EasyFarm.ViewModels
{
    public sealed class StringSink : BindableBase, IObserver<EventEntry>
    {
        private readonly Action<String> _action;

        public StringSink(Action<String> action)
        {
            _action = action;
        }

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(EventEntry value)
        {
            var timestamp = GetTimeStamp(value.Timestamp);
            var message = String.Join(" ", value.Payload);
            _action(timestamp + " " + message);
        }

        public String GetTimeStamp(DateTimeOffset time)
        {
            return String.Join(":", time.Hour, time.Minute, time.Second);
        }
    }
}