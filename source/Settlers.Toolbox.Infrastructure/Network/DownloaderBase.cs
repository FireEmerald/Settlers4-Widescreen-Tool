using System.Net;
using System.Text;

namespace Settlers.Toolbox.Infrastructure.Network
{
    public abstract class DownloaderBase
    {
        protected WebClient InitializeWebClient()
        {
            var client = new WebClient
            {
                Encoding = Encoding.UTF8,
                UseDefaultCredentials = true
            };

            client.Headers.Add(HttpRequestHeader.UserAgent, AssemblyInfo.FullName);

            return client;
        }
    }
}