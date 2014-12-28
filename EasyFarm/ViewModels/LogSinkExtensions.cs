using System;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;

namespace EasyFarm.ViewModels
{
    public static class LogSinkExtensions
    {
        public static SinkSubscription<StringSink> LogToCollection(
            this IObservable<EventEntry> eventStream,
            Action<String> action,
            IEventTextFormatter formatter = null)
        {
            var sink = new StringSink(action);
            var subscription = eventStream.Subscribe(sink);
            return new SinkSubscription<StringSink>(subscription, sink);
        }
    }
}