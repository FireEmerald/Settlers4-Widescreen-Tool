using Serilog.Events;

namespace Settlers.Toolbox.Infrastructure.Logging.Interfaces
{
    public interface ILogLevelConverter
    {
        LogEventLevel Convert(LogLevel logLevel);
    }
}