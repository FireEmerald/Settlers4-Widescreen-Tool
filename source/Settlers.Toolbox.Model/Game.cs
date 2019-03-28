using System;
using System.Diagnostics;
using System.IO;

using Settlers.Toolbox.Infrastructure;
using Settlers.Toolbox.Infrastructure.ExtensionMethods;
using Settlers.Toolbox.Infrastructure.Logging;
using Settlers.Toolbox.Model.Addons;
using Settlers.Toolbox.Model.Addons.Interfaces;
using Settlers.Toolbox.Model.Compatibility.Interfaces;
using Settlers.Toolbox.Model.Interfaces;
using Settlers.Toolbox.Model.Languages;
using Settlers.Toolbox.Model.Languages.Interfaces;
using Settlers.Toolbox.Model.Resolutions;
using Settlers.Toolbox.Model.Resolutions.Interfaces;
using Settlers.Toolbox.Model.Textures.Interfaces;

namespace Settlers.Toolbox.Model
{
    public class Game : IGame
    {
        private const string SETTLERS_PROC_NAME = "S4";
        private const string SETTLERS_S4_EXE = SETTLERS_PROC_NAME + ".exe";

        private const string SETTLERS_S4_MAIN_RELATIVE_PATH = @"Exe\S4_Main.exe";

        private const string S4_MAIN_GOG_SHA1 = "E23CD82EDD8783368BA8BE0DDD322E1AE44B5E6C";
        private const int    S4_MAIN_RETAIL_MAJOR_VERSION_NUMBER = 1;

        // Cached values
        private GameAddon _CachedInstalledAddons;

        private readonly IAddonManager _AddonManager;
        private readonly IResolutionChanger _ResolutionChanger;
        private readonly ILanguageChanger _LanguageChanger;
        private readonly ICompatibilityManager _CompatibilityManager;
        private readonly ITextureSwapper _TextureSwapper;

        public event Action<Result> LanguageChangeCompleted;

        public Game(IAddonManager addonManager, IResolutionChanger resolutionChanger, ILanguageChanger languageChanger,
            ICompatibilityManager compatibilityManager, ITextureSwapper textureSwapper)
        {
            if (resolutionChanger == null) throw new ArgumentNullException(nameof(resolutionChanger));
            if (languageChanger == null) throw new ArgumentNullException(nameof(languageChanger));
            if (compatibilityManager == null) throw new ArgumentNullException(nameof(compatibilityManager));
            if (textureSwapper == null) throw new ArgumentNullException(nameof(textureSwapper));

            _AddonManager = addonManager;
            _ResolutionChanger = resolutionChanger;
            _LanguageChanger = languageChanger;
            _CompatibilityManager = compatibilityManager;
            _TextureSwapper = textureSwapper;

            LatestVersion = new Version("2.50.1516.0");

            // Link events
            _LanguageChanger.LanguageChangeCompleted += HandleLanguageChangeCompleted;
        }

        ~Game()
        {
            // Unlink events
            _LanguageChanger.LanguageChangeCompleted -= HandleLanguageChangeCompleted;
        }

        public DirectoryInfo InstallDir { get; set; }

        public GameLanguage Language => IsInstalled ? _LanguageChanger.DetectLanguage(InstallDir) : GameLanguage.Unknown;

        public Resolution Resolution => IsInstalled ? _ResolutionChanger.DetectResolution(InstallDir) : null;

        public GameAddon InstalledAddons => IsInstalled ? _CachedInstalledAddons : GameAddon.None;

        public GameModule SupportedModules
        {
            get
            {
                if (!IsInstalled)
                    return GameModule.None;

                var modules = GameModule.Resolution | GameModule.CompatibilityFix; // Modules which are always enabled.

                if (IsGogVersion)
                    modules |= GameModule.Language;

                if (InstalledAddons.HasFlag(GameAddon.DieNeueWelt))
                    modules |= GameModule.TexturePack;

                return modules;
            }
        }

        public Version LatestVersion { get; }

        public Version InstalledVersion
        {
            get
            {
                if (!IsInstalled)
                    return null;

                string s4MainFilePath = Path.Combine(InstallDir.FullName, SETTLERS_S4_MAIN_RELATIVE_PATH);
                var s4MainVersionInfo = FileVersionInfo.GetVersionInfo(s4MainFilePath);

                return new Version(s4MainVersionInfo.FileMajorPart, s4MainVersionInfo.FileMinorPart,
                                   s4MainVersionInfo.FileBuildPart, s4MainVersionInfo.FilePrivatePart);
            }
        }

        public bool IsGogVersion
        {
            get
            {
                if (!IsInstalled)
                    return false;

                string s4MainFilePath = Path.Combine(InstallDir.FullName, SETTLERS_S4_MAIN_RELATIVE_PATH);
                var s4MainFile = new FileInfo(s4MainFilePath);

                return s4MainFile.HashEquals(S4_MAIN_GOG_SHA1);
            }
        }

        public bool IsRetailVersion => InstalledVersion != null && InstalledVersion.Major == S4_MAIN_RETAIL_MAJOR_VERSION_NUMBER;

        public bool IsCompatibilityFixApplied => IsInstalled && _CompatibilityManager.IsFixApplied(InstallDir);

        public bool IsTexturePackActive => IsInstalled && _TextureSwapper.IsTropicalTexturePackActive(InstallDir);

        public bool IsInstalled => InstallDir != null && InstallDir.Exists;

        public bool IsRunning => Process.GetProcessesByName(SETTLERS_PROC_NAME).Length > 0;

        public void RefreshInstalledAddons()
        {
            if (IsInstalled)
                _CachedInstalledAddons = _AddonManager.DetectInstalledAddons(InstallDir);
        }

        public bool IsAddonDisk(GameAddon gameAddon, DriveInfo driveInfo)
        {
            return _AddonManager.IsAddonDisk(gameAddon, driveInfo);
        }

        public Result InstallAddon(GameAddon gameAddon,DriveInfo addonDisk)
        {
            return _AddonManager.InstallAddon(gameAddon, InstallDir, addonDisk);
        }

        public void ApplyCompatibilityFix()
        {
            _CompatibilityManager.ApplyFix(InstallDir);
        }

        public void RemoveCompatibilityFix()
        {
            _CompatibilityManager.RemoveFix(InstallDir);
        }

        public void ActivateTropicalTextures()
        {
            _TextureSwapper.ActivateTropicalTextures(InstallDir);
        }

        public void DeactivateTropicalTextures()
        {
            _TextureSwapper.DeactivateTropicalTextures(InstallDir);
        }

        public void ChangeResolution(GameResolution gameResolution)
        {
            _ResolutionChanger.ChangeResolution(InstallDir, gameResolution);
        }

        public void ChangeResolution(ushort width, ushort height)
        {
            _ResolutionChanger.ChangeResolution(InstallDir, width, height);
        }

        public void ChangeLanguage(GameLanguage language)
        {
            _LanguageChanger.ChangeLanguage(InstallDir, language);
        }

        private void HandleLanguageChangeCompleted(Result result)
        {
            LanguageChangeCompleted?.Invoke(result);
        }

        public void Start()
        {
            try
            {
                Process.Start(Path.Combine(InstallDir.FullName, SETTLERS_S4_EXE));
            }
            catch (Exception ex)
            {
                LogAdapter.Log(LogLevel.Error, ex.Message);
                LogAdapter.Log(LogLevel.Warning, "Unable to start the game, a error occurred.");
            }
        }
    }
}