// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
// 
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
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
