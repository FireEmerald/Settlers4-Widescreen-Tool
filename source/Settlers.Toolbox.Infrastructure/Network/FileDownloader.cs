using System;
using System.ComponentModel;
using System.IO;
using System.Net;

using Settlers.Toolbox.Infrastructure.ExtensionMethods;
using Settlers.Toolbox.Infrastructure.Network.Interfaces;

namespace Settlers.Toolbox.Infrastructure.Network
{
    public class FileDownloader : DownloaderBase, IFileDownloader
    {
        public event Action DownloadSuccessful;
        public event Action<string> DownloadFailed;
        public event Action<string> DownloadProgressUpdate;

        private int _LastProgressPercentage;

        public FileDownloader()
        {
            _LastProgressPercentage = 0;
        }

        public void DownloadFile(string fileUrl, FileInfo destinationFile)
        {
            if (string.IsNullOrEmpty(fileUrl)) throw new ArgumentNullException(nameof(fileUrl));
            if (destinationFile == null) throw new ArgumentNullException(nameof(destinationFile));

            if (destinationFile.Exists)
                throw new IOException($"Unable to download file '{fileUrl}', destination file does already exist '{destinationFile.FullName}'.");

            WebClient client = InitializeWebClient();

            client.DownloadFileCompleted += HandleDownloadFileCompleted;
            client.DownloadProgressChanged += HandleDownloadProgressChanged;

            _LastProgressPercentage = 0;

            var fileUri = new Uri(fileUrl);
            client.DownloadFileAsync(fileUri, destinationFile.FullName);
        }

        private void HandleDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                DownloadFailed?.Invoke(e.Error.Message);
            }
            else
            {
                DownloadSuccessful?.Invoke();
            }
        }

        private void HandleDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Only trigger on 1% steps
            if (_LastProgressPercentage != e.ProgressPercentage)
            {
                _LastProgressPercentage = e.ProgressPercentage;

                DownloadProgressUpdate?.Invoke($"{e.ProgressPercentage}% Downloaded, {e.BytesReceived.AsBytesToMegabytes():0.00} MB of {e.TotalBytesToReceive.AsBytesToMegabytes():0.00} MB.");
            }
        }
    }
}