namespace Settlers.Toolbox.Infrastructure.Logging
{
    public static class LogAdapter
    {
        public static void Log(LogLevel logLevel, string message)
        {
            LogManager.Instance.CurrentLogger.Log(logLevel, message);
        }
    }
}