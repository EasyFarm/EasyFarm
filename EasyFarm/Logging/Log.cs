using System;
using System.Collections.ObjectModel;
using System.Linq;
using NLog;
using NLog.Config;
using System.Threading;
using System.Windows;

namespace EasyFarm.Logging
{
    public class Log
    {
        private static Logger _logger;

        public static ObservableCollection<string> LoggedItems = new ObservableCollection<string>();

        public static void Initialize()
        {
            LogManager.ThrowExceptions = true;
            var config = new LoggingConfiguration();
            var target = new LogSink(PublishLogItem) { Layout = @"${date:format=HH\:mm\:ss} ${message}" };
            config.AddTarget("LogSink", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, target));
            LogManager.Configuration = config;
            _logger = LogManager.GetLogger("LogSink");
        }

        public static void Write(string message)
        {
            _logger.Info(message);
        }

        /// <summary>
        ///     Publish log item under the right thread context.
        /// </summary>
        /// <param name="message"></param>
        public static void PublishLogItem(string message)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                AddLogItem(message);
            });
        }

        /// <summary>
        /// Add message to log without while preventing a <see cref="OutOfMemoryException"/>. 
        /// </summary>
        /// <param name="message"></param>
        public static void AddLogItem(string message)
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
