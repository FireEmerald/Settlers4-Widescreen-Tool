using System;

using Settlers.Toolbox.Infrastructure.Reporting.Interfaces;

namespace Settlers.Toolbox.Infrastructure.Reporting
{
    public class ReportManager : IReportManager
    {
        public event Action<string> ReportReceived;
        public event Action<string> StatusReportReceived;

        public void ReportMessage(string message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            RaiseReportReceived(message);
        }

        public void ReportStatus(string status)
        {
            if (status == null) throw new ArgumentNullException(nameof(status));

            RaiseStatusReportReceived(status);
        }

        private void RaiseReportReceived(string message)
        {
            ReportReceived?.Invoke(message);
        }

        private void RaiseStatusReportReceived(string statusMessage)
        {
            StatusReportReceived?.Invoke(statusMessage);
        }
    }
}