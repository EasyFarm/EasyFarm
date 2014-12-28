using EasyFarm.Logging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;

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
            LoggedItems = new ObservableCollection<string>();
            EventListener = new ObservableEventListener();
            EventListener.EnableEvents(Logger.Write, EventLevel.Verbose);
            EventListener.LogToCollection(x => LoggedItems.Add(x));
        }
    }
}