using System;

using Serilog.Events;

using Settlers.Toolbox.Infrastructure.Logging.Interfaces;

namespace Settlers.Toolbox.Infrastructure.Logging
{
    public class LogLevelConverter : ILogLevelConverter
    {
        public LogEventLevel Convert(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Verbose:
                    return LogEventLevel.Verbose;
                case LogLevel.Debug:
                    return LogEventLevel.Debug;
                case LogLevel.Information:
                    return LogEventLevel.Information;
                case LogLevel.Warning:
                    return LogEventLevel.Warning;
                case LogLevel.Error:
                    return LogEventLevel.Error;
                case LogLevel.Fatal:
                    return LogEventLevel.Fatal;
                default:
                    throw new InvalidOperationException($"Unable to convert {logLevel}, not implemented.");
            }
        }
    }
}