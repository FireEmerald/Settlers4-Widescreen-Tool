using System.IO;

using Settlers.Toolbox.Infrastructure;

namespace Settlers.Toolbox.Model.Addons.Interfaces
{
    public interface IAddonManager
    {
        GameAddon DetectInstalledAddons(DirectoryInfo installDir);

        bool IsAddonDisk(GameAddon gameAddon, DriveInfo driveInfo);
        Result InstallAddon(GameAddon gameAddon, DirectoryInfo installDir, DriveInfo addonDisk);
    }
}