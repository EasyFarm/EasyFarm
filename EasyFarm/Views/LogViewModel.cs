
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
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Threading;

namespace EasyFarm.ViewModels
{
    [ViewModelAttribute("Log")]
    public class LogViewModel : ViewModelBase
    {
        public ObservableCollection<String> LoggedItems { get; set; }

        public StringSink EventSink { get; set; }

        public ObservableEventListener EventListener { get; set; }

        private readonly SynchronizationContext _syncContext;

        public LogViewModel()
        {
            LoggedItems = new ObservableCollection<string>();
            EventListener = new ObservableEventListener();
            EventListener.EnableEvents(Logger.Write, EventLevel.Verbose);
            this._syncContext = SynchronizationContext.Current;
            this.EventListener.LogToCollection(PublishLogItem);
            // Can only be called on the dispatcher's thread. 
        }

        /// <summary>
        /// Publish log item under the right thread context. 
        /// </summary>
        /// <param name="message"></param>
        public void PublishLogItem(String message)
        {
            if (this._syncContext == SynchronizationContext.Current)
                LoggedItems.Add(message);
            else
                _syncContext.Send(o => LoggedItems.Add(message), null);
        }
    }
}