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

using System.Diagnostics.Tracing;

namespace EasyFarm.Logging
{
    /// <summary>
    ///     The logger for logging events to file or other mediums.
    /// </summary>
    public class Logger : EventSource
    {
        /// <summary>
        ///     Our internal Write of our logger.
        /// </summary>
        public static Logger Write = new Logger();

        /// <summary>
        ///     Logs the program's start.
        /// </summary>
        /// <param name="message"></param>
        [Event(EventId.ApplicationStart, Level = EventLevel.Informational)]
        public void ApplicationStart(string message)
        {
            SimpleWrite(EventId.ApplicationStart, message);
        }

        /// <summary>
        ///     Logs the program's end.
        /// </summary>
        /// <param name="message"></param>
        [Event(EventId.ApplicationEnd, Level = EventLevel.Informational)]
        public void ApplicationEnd(string message)
        {
            SimpleWrite(EventId.ApplicationEnd, message);
        }

        /// <summary>
        ///     Logs the success of finding all resource folders / files.
        /// </summary>
        /// <param name="message"></param>
        [Event(EventId.ResourcesLocated, Level = EventLevel.Informational)]
        public void ResourcesLocated(string message)
        {
            SimpleWrite(EventId.ResourcesLocated, message);
        }

        /// <summary>
        ///     Logs events for resource files being missing.
        /// </summary>
        /// <param name="message"></param>
        [Event(EventId.ResourceFilesMissing, Level = EventLevel.Error)]
        public void ResourceFileMissing(string message)
        {
            SimpleWrite(EventId.ResourceFilesMissing, message);
        }

        /// <summary>
        ///     Logs events for the resources folder being missing.
        /// </summary>
        /// <param name="message"></param>
        [Event(EventId.ResourceFolderMissing, Level = EventLevel.Error)]
        public void ResourceFolderMissing(string message)
        {
            SimpleWrite(EventId.ResourceFolderMissing, message);
        }

        [Event(EventId.ProcessNotFound, Level = EventLevel.Error)]
        public void ProcessNotFound(string message)
        {
            SimpleWrite(EventId.ProcessNotFound, message);
        }

        [Event(EventId.ProcessFound, Level = EventLevel.Error)]
        public void ProcessFound(string message)
        {
            SimpleWrite(EventId.ProcessFound, message);
        }

        [Event(EventId.BotStart, Level = EventLevel.Informational)]
        public void BotStart(string message)
        {
            SimpleWrite(EventId.BotStart, message);
        }

        [Event(EventId.BotStop, Level = EventLevel.Informational)]
        public void BotStop(string message)
        {
            SimpleWrite(EventId.BotStop, message);
        }

        [Event(EventId.SettingsSave, Level = EventLevel.Informational)]
        public void SaveSettings(string message)
        {
            SimpleWrite(EventId.SettingsSave, message);
        }

        [Event(EventId.StateCheck, Level = EventLevel.Informational)]
        public void StateCheck(string message, bool success)
        {
            SimpleWrite(EventId.StateCheck, message);
        }

        [Event(EventId.StateRun, Level = EventLevel.Informational)]
        public void StateRun(string message)
        {
            SimpleWrite(EventId.StateRun, message);
        }

        [Event(EventId.PerformanceElapsedTime, Level = EventLevel.Informational)]
        public void PerformanceElapsedTime(string message)
        {
            SimpleWrite(EventId.PerformanceElapsedTime, message);
        }

        public void SimpleWrite(int id, string message)
        {
            if (IsEnabled())
            {
                WriteEvent(id, message);
            }
        }

        /// <summary>
        ///     Stores all the EventIDs for events in the application.
        ///     We're using this instead of an enum to avoid conversion of
        ///     enum elements to ints 100 times..
        /// </summary>
        public class EventId
        {
            // Application: 0 - 99
            public const int ApplicationStart = 1;
            public const int ApplicationEnd = 2;
            // Resources: 100 - 199
            public const int ResourcesLocated = 100;
            public const int ResourceFolderMissing = 101;
            public const int ResourceFilesMissing = 102;
            // Processes: 200 - 299
            public const int ProcessFound = 200;
            public const int ProcessNotFound = 201;
            // Bot: 300 - 399
            public const int BotStart = 300;
            public const int BotStop = 301;
            // Settings: 400 - 499
            public const int SettingsSave = 400;
            public const int SettingsLoad = 401;
            // Settings: 500 - 599
            public const int StateCheck = 500;
            public const int StateEnter = 501;
            public const int StateRun = 502;
            public const int StateExit = 503;
            // Performance: 600-699
            public const int PerformanceElapsedTime = 600;
        }
    }
}