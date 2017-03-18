using System;

namespace EasyFarm.Logging
{
    public class LogEntry
    {
        public readonly LoggingEventType Severity;
        public readonly string Message;
        public readonly Exception Exception;

        public LogEntry(LoggingEventType severity, string message, Exception exception = null)
        {
            Severity = severity;
            Message = message;
            Exception = exception;
        }

        public LogEntry IncludeExceptionInMessage()
        {
            if (Exception == null) return this;

            var exceptionMessage = Exception.ToString();

            var extendedMessage = string.Join(
                Environment.NewLine,
                Message,
                exceptionMessage);

            return new LogEntry(Severity, extendedMessage, Exception);
        }
    }
}