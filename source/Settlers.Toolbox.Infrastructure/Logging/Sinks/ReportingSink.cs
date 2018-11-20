using System;

using Serilog.Core;
using Serilog.Events;

using Settlers.Toolbox.Infrastructure.Reporting.Interfaces;

namespace Settlers.Toolbox.Infrastructure.Logging.Sinks
{
    public class ReportingSink : ILogEventSink
    {
        private readonly IReportManager _ReportManager;
        private readonly IFormatProvider _FormatProvider;

        public ReportingSink(IReportManager reportManager, IFormatProvider formatProvider = null)
        {
            if (reportManager == null) throw new ArgumentNullException(nameof(reportManager));

            _ReportManager = reportManager;
            _FormatProvider = formatProvider;
        }

        public void Emit(LogEvent logEvent)
        {
            string message = logEvent.RenderMessage(_FormatProvider);
            _ReportManager.ReportMessage(message);
        }
    }
}