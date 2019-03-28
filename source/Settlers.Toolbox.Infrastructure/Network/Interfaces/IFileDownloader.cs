using System;
using System.IO;

namespace Settlers.Toolbox.Infrastructure.Network.Interfaces
{
    public interface IFileDownloader
    {
        event Action DownloadSuccessful;
        event Action<string> DownloadFailed;
        event Action<string> DownloadProgressUpdate;

        void DownloadFile(string fileUrl, FileInfo destinationFile);
    }
}