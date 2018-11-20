using Settlers.Toolbox.Infrastructure.Logging.Logger.Interfaces;

namespace Settlers.Toolbox.Infrastructure.Logging.Logger
{
    public class NullLogger : IInternalLogger
    {
        public void Log(LogLevel logLevel, string message)
        {
            // Nothing to do.
        }
    }
}