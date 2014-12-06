using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Logging
{
    /// <summary>
    /// The logger for logging events to file or other mediums. 
    /// </summary>
    public class Logger : EventSource
    {
        /// <summary>
        /// Stores all the EventIDs for events in the application. 
        /// We're using this instead of an enum to avoid conversion of 
        /// enum elements to ints 100 times.. 
        /// </summary>
        private class EventID
        {
            // Application: 0 - 99
            public const int APPLICATION_START = 1;
            public const int APPLICATION_END = 2;

            // Resources: 100 - 199
            public const int RESOURCES_LOCATED = 100;
            public const int RESOURCE_FOLDER_MISSING = 101;
            public const int RESOURCE_FILES_MISSING = 102;

            // Processes: 200 - 299
            public const int PROCESS_FOUND = 200;
            public const int PROCESS_NOT_FOUND = 201;

            // Bot: 300 - 399
            public const int BOT_START = 300;
            public const int BOT_STOP = 301;

            // Settings: 400 - 499
            public const int SETTINGS_SAVE = 400;
            public const int SETTINGS_LOAD = 401;

        }

        /// <summary>
        /// Our internal instance of our logger. 
        /// </summary>
        private readonly static Lazy<Logger> m_instance =
            new Lazy<Logger>(() => new Logger());

        /// <summary>
        /// Returns a static instance to our logger object for 
        /// writing log messages. 
        /// </summary>
        public static Logger Write
        {
            get { return m_instance.Value; }
        }

        /// <summary>
        /// Logs the program's start. 
        /// </summary>
        /// <param name="message"></param>
        [Event(EventID.APPLICATION_START, Level = EventLevel.Informational)]
        public void ApplicationStart(string message)
        {
            if (this.IsEnabled()) WriteEvent(EventID.APPLICATION_START, message);
        }

        /// <summary>
        /// Logs the program's end. 
        /// </summary>
        /// <param name="message"></param>
        [Event(EventID.APPLICATION_END, Level = EventLevel.Informational)]
        public void ApplicationEnd(string message)
        {
            if (this.IsEnabled()) WriteEvent(EventID.APPLICATION_END, message);
        }

        /// <summary>
        /// Logs the success of finding all resource folders / files. 
        /// </summary>
        /// <param name="message"></param>
        [Event(EventID.RESOURCES_LOCATED, Level = EventLevel.Informational)]
        public void ResourcesLocated(string message)
        {
            if (this.IsEnabled()) WriteEvent(EventID.RESOURCES_LOCATED, message);
        }

        /// <summary>
        /// Logs events for resource files being missing. 
        /// </summary>
        /// <param name="message"></param>
        [Event(EventID.RESOURCE_FILES_MISSING, Level = EventLevel.Error)]
        public void ResourceFileMissing(string message)
        {
            if (this.IsEnabled()) WriteEvent(EventID.RESOURCE_FILES_MISSING, message);
        }

        /// <summary>
        /// Logs events for the resources folder being missing. 
        /// </summary>
        /// <param name="message"></param>
        [Event(EventID.RESOURCE_FOLDER_MISSING, Level = EventLevel.Error)]
        public void ResourceFolderMissing(string message)
        {
            if (this.IsEnabled()) WriteEvent(EventID.RESOURCE_FOLDER_MISSING, message);
        }

        [Event(EventID.PROCESS_NOT_FOUND, Level = EventLevel.Error)]
        public void ProcessNotFound(string message)
        {
            if (this.IsEnabled()) WriteEvent(EventID.PROCESS_NOT_FOUND, message);
        }

        [Event(EventID.PROCESS_FOUND, Level = EventLevel.Error)]
        public void ProcessFound(string message)
        {
            if (this.IsEnabled()) WriteEvent(EventID.PROCESS_FOUND, message);
        }

        [Event(EventID.BOT_START, Level = EventLevel.Informational)]
        public void BotStart(string message)
        {
            if (this.IsEnabled()) WriteEvent(EventID.BOT_START, message);
        }

        [Event(EventID.BOT_STOP, Level = EventLevel.Informational)]
        public void BotStop(string message)
        {
            if (this.IsEnabled()) WriteEvent(EventID.BOT_STOP, message);
        }

        [Event(EventID.SETTINGS_SAVE, Level = EventLevel.Informational)]
        public void SaveSettings(string message)
        {
            if (this.IsEnabled()) WriteEvent(EventID.SETTINGS_SAVE, message);
        }
    }
}