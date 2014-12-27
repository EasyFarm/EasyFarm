
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

using EasyFarm.Logging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.ViewModels
{
    [ViewModelAttribute("Log")]
    public class LogViewModel : ViewModelBase
    {
        public ObservableCollection<String> LoggedItems { get; set; }

        public StringSink EventSink { get; set; }

        public ObservableEventListener EventListener { get; set; }

        public LogViewModel()
        {
            this.LoggedItems = new ObservableCollection<string>();
            this.EventListener = new ObservableEventListener();
            this.EventListener.EnableEvents(Logger.Write, EventLevel.Verbose);
            this.EventListener.LogToCollection((String x) => { this.LoggedItems.Add(x); });
        }
    }

    public sealed class StringSink : BindableBase, IObserver<EventEntry>
    {
        private IEventTextFormatter formatter;
        private Action<String> action;

        public StringSink(Action<String> action, IEventTextFormatter formatter)
        {
            this.formatter = formatter ?? new EventTextFormatter();
            this.action = action;
        }

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(EventEntry value)
        {
            using (var writer = new StringWriter())
            {
                // this.formatter.WriteEvent(value, writer);
                // this.action(writer.ToString());
                var timestamp = GetTimeStamp(value.Timestamp);
                var message = String.Join(" ", value.Payload);
                this.action(timestamp + " " + message);
            }
        }

        public String GetTimeStamp(DateTimeOffset time)
        {
            return String.Join(":", time.Hour, time.Minute, time.Second);
        }
    }

    public static class LogSinkExtensions
    {
        public static SinkSubscription<StringSink> LogToCollection(
            this IObservable<EventEntry> eventStream,
            Action<String> action,
            IEventTextFormatter formatter = null)
        {
            var sink = new StringSink(action, formatter);
            var subscription = eventStream.Subscribe(sink);
            return new SinkSubscription<StringSink>(subscription, sink);
        }
    }
}