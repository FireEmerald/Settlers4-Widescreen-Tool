using System;

namespace Settlers.Toolbox.Infrastructure.Reporting.Interfaces
{
    public interface IReportManager
    {
        event Action<string> ReportReceived;
        event Action<string> StatusReportReceived;

        void ReportMessage(string message);
        void ReportStatus(string status);
    }
}