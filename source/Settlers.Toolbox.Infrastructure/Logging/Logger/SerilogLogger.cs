using System;

using Serilog;
using Serilog.Core;
using Serilog.Events;

using Settlers.Toolbox.Infrastructure.Logging.Interfaces;
using Settlers.Toolbox.Infrastructure.Logging.Logger.Interfaces;

namespace Settlers.Toolbox.Infrastructure.Logging.Logger
{
    public class SerilogLogger : IInternalLogger
    {
        private readonly Serilog.Core.Logger _Logger;
        private readonly ILogLevelConverter _LogLevelConverter;

        public SerilogLogger(ILogEventSink usedSink, ILogLevelConverter logLevelConverter)
        {
            if (usedSink == null) throw new ArgumentNullException(nameof(usedSink));
            if (logLevelConverter == null) throw new ArgumentNullException(nameof(logLevelConverter));

            _LogLevelConverter = logLevelConverter;

            _Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Sink(usedSink)
                .CreateLogger();
        }

        public void Log(LogLevel logLevel, string message)
        {
            LogEventLevel logEventLevel = _LogLevelConverter.Convert(logLevel);
            _Logger.Write(logEventLevel, message);
        }
    }
}