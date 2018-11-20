using System.IO;
using System.IO.Compression;

using Settlers.Toolbox.Infrastructure.IO.Interfaces;

namespace Settlers.Toolbox.Infrastructure.IO
{
    public class ZipFileAdapter : IZipFileAdapter
    {
        public void ExtractToDirectory(FileInfo fileToUnzip, DirectoryInfo destinationDirectory)
        {
            ZipFile.ExtractToDirectory(fileToUnzip.FullName, destinationDirectory.FullName);
        }
    }
}