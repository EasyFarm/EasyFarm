namespace EasyFarm.Logging
{
    public static class Logger
    {
        public static ILogger Instance { get; set; } = new NLogTextFileLogger();

        public static void Log(LogEntry logEntry)
        {
            Instance.Log(logEntry);
        }
    }
}