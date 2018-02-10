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
using System.Collections.ObjectModel;
using EasyFarm.Classes;
using EasyFarm.Infrastructure;

namespace EasyFarm.ViewModels
{
    public class LogViewModel : ViewModelBase
    {
        private static readonly LogEntries LogEntries = new LogEntries();
        private static readonly object LockObject = new object();

        public LogViewModel()
        {
            ViewName = "Log";
        }

        public ObservableCollection<string> LoggedItems
        {
            get { return LogEntries.LoggedItems; }
            set { Set(ref LogEntries.LoggedItems, value); }
        }

        public static void Write(string message)
        {
            lock (LockObject)
            {
                LogEntries.Write(message);
            }
        }
    }
}