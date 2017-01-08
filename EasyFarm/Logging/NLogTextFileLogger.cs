using System;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace EasyFarm.Logging
{
    public class NLogTextFileLogger : ILogger
    {
        private readonly NLog.Logger _logger = LogManager.GetLogger("LogSink");

        static NLogTextFileLogger()
        {
            InitializeLogging();
        }

        private static void InitializeLogging()
        {
            LogManager.ThrowExceptions = true;
            var config = new LoggingConfiguration();
            var target = new FileTarget() { FileName = "EasyFarm.log" };
            config.AddTarget("LogSink", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, target));
            LogManager.Configuration = config;
        }

        public void Log(LogEntry logEntry)
        {
            var message = string.Join(
                Environment.NewLine, 
                logEntry.Message, 
                logEntry.Exception?.ToString());

            _logger.Fatal(logEntry.Exception, message);
        }
    }
}