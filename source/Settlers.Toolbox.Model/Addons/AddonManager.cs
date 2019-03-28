using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Settlers.Toolbox.Infrastructure;
using Settlers.Toolbox.Model.Addons.Interfaces;

namespace Settlers.Toolbox.Model.Addons
{
    public class AddonManager : IAddonManager
    {
        private readonly IReadOnlyList<IAddon> _AllAddons;

        public AddonManager(IReadOnlyList<IAddon> allAddons)
        {
            if (allAddons == null || !allAddons.Any()) throw new ArgumentNullException(nameof(allAddons));

            _AllAddons = allAddons;
        }

        public GameAddon DetectInstalledAddons(DirectoryInfo installDir)
        {
            if (installDir == null) throw new ArgumentNullException(nameof(installDir));

            GameAddon installedAddons = GameAddon.None;

            foreach (IAddon addon in _AllAddons)
            {
                if (addon.IsInstalled(installDir))
                    installedAddons |= addon.Id;
            }

            return installedAddons;
        }

        public bool IsAddonDisk(GameAddon gameAddon, DriveInfo driveInfo)
        {
            if (driveInfo == null) throw new ArgumentNullException(nameof(driveInfo));

            IAddon addonToCheck = _AllAddons.Single(addon => addon.Id == gameAddon);

            return addonToCheck.IsAddonDisk(driveInfo);
        }

        public Result InstallAddon(GameAddon gameAddon, DirectoryInfo installDir, DriveInfo addonDisk)
        {
            if (installDir == null) throw new ArgumentNullException(nameof(installDir));
            if (addonDisk == null) throw new ArgumentNullException(nameof(addonDisk));

            IAddon addonToInstall = _AllAddons.Single(addon => addon.Id == gameAddon);

            return addonToInstall.InstallFromDisk(installDir, addonDisk);
        }
    }
}