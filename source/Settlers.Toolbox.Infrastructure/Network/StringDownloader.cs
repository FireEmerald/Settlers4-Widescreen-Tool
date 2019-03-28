using System;
using System.Net;

using Settlers.Toolbox.Infrastructure.Network.Interfaces;

namespace Settlers.Toolbox.Infrastructure.Network
{
    public class StringDownloader : DownloaderBase, IStringDownloader
    {
        public string DownloadString(string stringUrl)
        {
            if (string.IsNullOrEmpty(stringUrl)) throw new ArgumentNullException(nameof(stringUrl));

            WebClient client = InitializeWebClient();

            var retrievedString = string.Empty;
            try
            {
                retrievedString = client.DownloadString(stringUrl);
            }
            catch (WebException)
            {
                // Returning string.Empty is enough.
                // LogAdapter.Log(LogLevel.Error, ex.Message);
            }
            finally
            {
                client.Dispose();
            }

            return retrievedString;
        }
    }
}