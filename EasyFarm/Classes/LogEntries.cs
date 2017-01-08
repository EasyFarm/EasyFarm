using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace EasyFarm.Classes
{
    public class LogEntries
    {
        public ObservableCollection<string> LoggedItems = new ObservableCollection<string>();
        public Func<Dispatcher> DispatcherFactory { get; set; } = () => Application.Current.Dispatcher;

        public void Write(string message)
        {
            Write(DateTime.Now, message);
        }

        public void Write(DateTime dateTime, string message)
        {
            var timeStampedMessage = PrependTimestamp(dateTime, message);
            RecordLogItemOnUiThread(timeStampedMessage);
        }

        private string PrependTimestamp(DateTime dateTime, string message)
        {
            var timeStamp = FormatTimeStamp(dateTime);
            return $"{timeStamp} {message}";
        }

        private string FormatTimeStamp(DateTime dateTime)
        {
            var timeStamp = dateTime.ToString("hh:mm:ss");
            return timeStamp;
        }

        private void RecordLogItemOnUiThread(string message)
        {
            var dispatcher = DispatcherFactory();
            dispatcher.Invoke(delegate { RecordLogItem(message); });
        }

        private void RecordLogItem(string message)
        {
            LoggedItems.Add(message);

            // Limit list to only 1000 items: prevent system out of memory exception. 
            if (LoggedItems.Count > 1000)
            {
                LoggedItems.Remove(LoggedItems.Last());
            }
        }
    }
}
