using System.Collections.Generic;
using System.IO;

using Settlers.Toolbox.Infrastructure;

namespace Settlers.Toolbox.Model.Addons.Interfaces
{
    public interface IAddon
    {
        GameAddon Id { get; }

        /// <summary>
        /// Dictionary &lt;FileName, RelativeFilePathInInstallDir&gt;
        /// </summary>
        IDictionary<string, string> FileMappings { get; }
        
        bool IsInstalled(DirectoryInfo installDir);
        bool IsAddonDisk(DriveInfo driveInfo);

        Result InstallFromDisk(DirectoryInfo installDir, DriveInfo addonDisk);
    }
}