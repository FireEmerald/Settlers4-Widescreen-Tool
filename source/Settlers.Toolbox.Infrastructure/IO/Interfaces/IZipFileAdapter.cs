using System.IO;

namespace Settlers.Toolbox.Infrastructure.IO.Interfaces
{
    public interface IZipFileAdapter
    {
        void ExtractToDirectory(FileInfo fileToUnzip, DirectoryInfo destinationDirectory);
    }
}