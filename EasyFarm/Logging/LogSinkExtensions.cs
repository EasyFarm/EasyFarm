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

using System;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;

namespace EasyFarm.Logging
{
    public static class LogSinkExtensions
    {
        public static SinkSubscription<StringSink> LogToCollection(
            this IObservable<EventEntry> eventStream,
            Action<string> action,
            IEventTextFormatter formatter = null)
        {
            var sink = new StringSink(action);
            var subscription = eventStream.Subscribe(sink);
            return new SinkSubscription<StringSink>(subscription, sink);
        }
    }
}