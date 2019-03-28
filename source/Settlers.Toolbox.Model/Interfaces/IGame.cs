using System;
using System.IO;

using Settlers.Toolbox.Infrastructure;
using Settlers.Toolbox.Model.Addons;
using Settlers.Toolbox.Model.Languages;
using Settlers.Toolbox.Model.Resolutions;

namespace Settlers.Toolbox.Model.Interfaces
{
    public interface IGame
    {
        event Action<Result> LanguageChangeCompleted;

        DirectoryInfo InstallDir { get; set; }

        GameLanguage Language { get; }
        Resolution Resolution { get; }

        GameAddon InstalledAddons { get; }
        GameModule SupportedModules { get; }

        Version LatestVersion { get; }
        Version InstalledVersion { get; }

        bool IsGogVersion { get; }
        bool IsRetailVersion { get; }

        bool IsCompatibilityFixApplied { get; }
        bool IsTexturePackActive { get; }

        bool IsInstalled { get; }
        bool IsRunning { get; }

        void RefreshInstalledAddons();

        bool IsAddonDisk(GameAddon gameAddon, DriveInfo driveInfo);
        Result InstallAddon(GameAddon gameAddon, DriveInfo addonDisk);

        void ApplyCompatibilityFix();
        void RemoveCompatibilityFix();

        void ActivateTropicalTextures();
        void DeactivateTropicalTextures();

        void ChangeResolution(GameResolution gameResolution);
        void ChangeResolution(ushort width, ushort height);

        void ChangeLanguage(GameLanguage language);

        void Start();
    }
}