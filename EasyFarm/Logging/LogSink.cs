using System;
using NLog;
using NLog.Config;
using NLog.Targets;
using Prism.Mvvm;

namespace EasyFarm.Logging
{
    [Target("LogSink")]
    public sealed class LogSink : TargetWithLayout
    {
        public Action<string> Action { get; set; }

        public LogSink(Action<string> action)
        {
            Action = action;
        }

        protected override void Write(LogEventInfo logEvent)
        {
            var message = this.Layout.Render(logEvent);
            Action($"{message}");
        }        
    }
}