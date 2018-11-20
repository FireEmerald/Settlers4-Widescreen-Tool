using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using FluentValidation.Results;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

using Microsoft.Win32;

using Settlers.Toolbox.Infrastructure;
using Settlers.Toolbox.Infrastructure.Logging;
using Settlers.Toolbox.Infrastructure.Logging.Logger.Interfaces;
using Settlers.Toolbox.Infrastructure.Reporting.Interfaces;
using Settlers.Toolbox.Interfaces;
using Settlers.Toolbox.Model;
using Settlers.Toolbox.Model.Addons;
using Settlers.Toolbox.Model.Interfaces;
using Settlers.Toolbox.Model.Languages;
using Settlers.Toolbox.Model.Resolutions;
using Settlers.Toolbox.Model.Updates;
using Settlers.Toolbox.Model.Updates.Interfaces;
using Settlers.Toolbox.ViewModels.Interfaces;
using Settlers.Toolbox.ViewModels.Validator.Interfaces;

namespace Settlers.Toolbox.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel, IDataErrorInfo
    {
        private const string NONE_ERROR = "";

        private GameResolution _SelectedResolution;
        private GameLanguage _SelectedLanguage;

        private string _SettlersExePath;

        private string _CustomWidth;
        private string _CustomHeight;

        private bool _ApplyCompatibilityFix;
        private bool _ActivateTexturePack;
        private bool _IsCustomResolution;

        private bool _IsPatching;

        private string _InformationText;
        private string _StatusMessageText;

        private readonly IMainViewModelValidator _MainViewModelValidator;
        private readonly IWindowManager _WindowManager;
        private readonly IGame _Game;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel(IMainViewModelValidator mainViewModelValidator, IWindowManager windowManager, IGame game,
            IReportManager reportManager, IInternalLogger internalLogger, IUpdateChecker updateChecker)
        {
            if (mainViewModelValidator == null) throw new ArgumentNullException(nameof(mainViewModelValidator));
            if (windowManager == null) throw new ArgumentNullException(nameof(windowManager));
            if (game == null) throw new ArgumentNullException(nameof(game));
            if (reportManager == null) throw new ArgumentNullException(nameof(reportManager));
            if (internalLogger == null) throw new ArgumentNullException(nameof(internalLogger));
            if (updateChecker == null) throw new ArgumentNullException(nameof(updateChecker));

            _MainViewModelValidator = mainViewModelValidator;
            _WindowManager = windowManager;
            _Game = game;

            InitializeEssentialComponents(reportManager, internalLogger);

            // Initialize controls
            PopulateCollections();

            OpenExeFileDialogCommand = new RelayCommand(HandleOpenExeFileDialog, () => !IsPatching);
            InstallDieNeueWeltCommand = new RelayCommand(HandleInstallDieNeueWelt, CanInstallDieNeueWelt);

            ShowReadmeWindowCommand = new RelayCommand(HandleShowReadmeWindow);
            ApplySettingsCommand = new RelayCommand(HandleApplySettings, CanApplySettings);
            StartGameCommand = new RelayCommand(HandleStartGame, CanStartGame);

            // Setup initial values
            InformationText = "Select your path, language, resolution and press 'Apply', then 'Start'." + Environment.NewLine;
            StatusMessageText = "Ready.";
            IsPatching = false;

            // Link events
            _Game.LanguageChangeCompleted += HandleLanguageChangeCompleted;

            //StartupRoutine(updateChecker);
        }

        ~MainViewModel()
        {
            // Unlink events
            _Game.LanguageChangeCompleted -= HandleLanguageChangeCompleted;
        }

        private bool CanInstallDieNeueWelt()
        {
            return !IsPatching && _Game.IsInstalled &&
                   _Game.IsGogVersion && !_Game.InstalledAddons.HasFlag(GameAddon.DieNeueWelt);
        }
        private bool CanApplySettings()
        {
            return !IsPatching && _Game.IsInstalled &&
                   (SelectedResolution != GameResolution.Custom || !AnyValidationFailed);
        }
        private bool CanStartGame()
        {
            return !IsPatching && _Game.IsInstalled;
        }

        public async void StartupRoutine(IUpdateChecker updateChecker)
        {
            StatusMessageText = "Checking for updates...";

            await Task.Delay(500); // Wait a sec for the UI.

            VersionPackage versionPackage = await updateChecker.RetrieveLatestUpdateOrNull();

            if (versionPackage == null)
            {
                StatusMessageText = "Ready.";
                _WindowManager.OpenReadmeWindow();

                return;
            }

            MessageBox.Show(versionPackage.DeltaChangelog + "This version is outdated, you will be redirected to download the current version.", "New version is available!",
                MessageBoxButton.OK, MessageBoxImage.Information);

            Process.Start(versionPackage.DownloadLink);
            Application.Current.Shutdown();
        }

        #region Initialize essential components

        private void InitializeEssentialComponents(IReportManager reportManager, IInternalLogger internalLogger)
        {
            // Exception handling
            AppDomain.CurrentDomain.UnhandledException += HandleUnhandledExceptions;

            // Setup reporting/logging
            reportManager.ReportReceived += HandleReportReceived;
            reportManager.StatusReportReceived += HandleStatusReportReceived;
            LogManager.Instance.CurrentLogger = internalLogger;
        }

        private void HandleUnhandledExceptions(object sender, UnhandledExceptionEventArgs args)
        {
            var ex = (Exception)args.ExceptionObject;
            MessageBox.Show(ex.ToString(), "Ooops! Unhandled exception occurred.", MessageBoxButton.OK, MessageBoxImage.Stop);
        }

        private void HandleReportReceived(string message)
        {
            InformationText += message + Environment.NewLine;
        }
        private void HandleStatusReportReceived(string statusMessage)
        {
            StatusMessageText = statusMessage;
        }
        #endregion
        
        private void PopulateCollections()
        {
            ResolutionCollection = new ObservableCollection<GameResolution>();
            foreach (var gameResolution in (GameResolution[])Enum.GetValues(typeof(GameResolution)))
            {
                if (gameResolution != GameResolution.Unknown)
                    ResolutionCollection.Add(gameResolution);
            }

            LanguageCollection = new ObservableCollection<GameLanguage>();
            foreach (var language in (GameLanguage[])Enum.GetValues(typeof(GameLanguage)))
            {
                if (language != GameLanguage.Unknown)
                    LanguageCollection.Add(language);
            }
        }

        public ObservableCollection<GameResolution> ResolutionCollection { get; private set; }
        public ObservableCollection<GameLanguage> LanguageCollection { get; private set; }

        public GameResolution SelectedResolution
        {
            get => _SelectedResolution;
            set
            {
                IsCustomResolution = value == GameResolution.Custom;

                _SelectedResolution = value;
                RaisePropertyChanged(nameof(SelectedResolution));
            }
        }
        public GameLanguage SelectedLanguage
        {
            get => _SelectedLanguage;
            set
            {
                _SelectedLanguage = value;
                RaisePropertyChanged(nameof(SelectedLanguage));
            }
        }

        public string SettlersExePath
        {
            get => _SettlersExePath;
            set
            {
                if (!string.IsNullOrEmpty(value) && value.EndsWith("S4.exe") && File.Exists(value))
                {
                    _Game.InstallDir = new FileInfo(value).Directory;
                    _Game.RefreshInstalledAddons();

                    InformationText = string.Empty;

                    NotifyUserAboutGameVersion();
                    UpdatePropertiesFromModel();
                }
                else
                {
                    _Game.InstallDir = null;
                }

                _SettlersExePath = value;
                RaisePropertyChanged(null); // Raise on all.
            }
        }

        private void NotifyUserAboutGameVersion()
        {
            if (!_Game.IsGogVersion)
            {
                if (_Game.IsRetailVersion)
                {
                    LogAdapter.Log(LogLevel.Warning, "The installed game is a retail version of Settlers 4.");
                }
                else
                {
                    if (_Game.InstalledVersion < _Game.LatestVersion)
                    {
                        // https://support.ubi.com/en-GB/Faqs/000010193/Where-can-i-find-the-Settlers-1V-patch-1364550240109
                        LogAdapter.Log(LogLevel.Warning, $"Your version {_Game.InstalledVersion} is outdated. There's a patch available to version {_Game.LatestVersion}. Download it now: http://patches.ubi.com/settlers_4/settlers4_v1xxx_to_1516a.exe");
                        LogAdapter.Log(LogLevel.Information, "------------------------------------------------------------------");
                    }

                    LogAdapter.Log(LogLevel.Warning, "The installed game is not a GOG version of Settlers 4.");
                    LogAdapter.Log(LogLevel.Warning, "Some features will remain locked e.g. language change.");
                }
            }
            else
            {
                LogAdapter.Log(LogLevel.Warning, "The installed game is a GOG version of Settlers 4.");
            }
        }

        private void UpdatePropertiesFromModel()
        {
            if (_Game.SupportedModules.HasFlag(GameModule.Language))
            {
                GameLanguage currentLanguage = _Game.Language;
                if (currentLanguage != GameLanguage.Unknown)
                {
                    SelectedLanguage = currentLanguage;
                }
                else
                {
                    LogAdapter.Log(LogLevel.Warning, "The current game language could not be determined.");
                }
            }

            if (_Game.SupportedModules.HasFlag(GameModule.Resolution))
            {
                Resolution currentResolution = _Game.Resolution;
                if (currentResolution != null)
                {
                    SelectedResolution = currentResolution.Id;

                    if (currentResolution.Id == GameResolution.Custom)
                    {
                        CustomWidth = currentResolution.Width.ToString();
                        CustomHeight = currentResolution.Height.ToString();
                    }
                }
                else
                {
                    LogAdapter.Log(LogLevel.Warning, "The current game resolution could not be determined.");
                }
            }

            ApplyCompatibilityFix = _Game.SupportedModules.HasFlag(GameModule.CompatibilityFix) &&
                                    _Game.IsCompatibilityFixApplied;

            ActivateTexturePack = _Game.SupportedModules.HasFlag(GameModule.TexturePack) &&
                                  _Game.IsTexturePackActive;
        }
        
        public string CustomWidth
        {
            get => _CustomWidth;
            set
            {
                _CustomWidth = value;
                RaisePropertyChanged(nameof(CustomWidth));
            }
        }
        public string CustomHeight
        {
            get => _CustomHeight;
            set
            {
                _CustomHeight = value;
                RaisePropertyChanged(nameof(CustomHeight));
            }
        }

        public string AddonStatusDieNeueWelt
        {
            get
            {
                if (!_Game.IsInstalled)
                    return "Unknown";

                return _Game.InstalledAddons.HasFlag(GameAddon.DieNeueWelt) ? "Installed" : "Not installed";
            }
        }

        public bool IsResolutionChangeSupported => !IsPatching && _Game.SupportedModules.HasFlag(GameModule.Resolution);
        public bool IsLanguageChangeSupported => !IsPatching && _Game.SupportedModules.HasFlag(GameModule.Language);
        public bool IsCompatibilityFixSupported => !IsPatching && _Game.SupportedModules.HasFlag(GameModule.CompatibilityFix);
        public bool IsTexturePackSupported => !IsPatching && _Game.SupportedModules.HasFlag(GameModule.TexturePack);

        public bool ApplyCompatibilityFix
        {
            get => _ApplyCompatibilityFix;
            set
            {
                _ApplyCompatibilityFix = value;
                RaisePropertyChanged(nameof(ApplyCompatibilityFix));
            }
        }

        public bool ActivateTexturePack
        {
            get => _ActivateTexturePack;
            set
            {
                _ActivateTexturePack = value;
                RaisePropertyChanged(nameof(ActivateTexturePack));
            }
        }

        public bool IsCustomResolution
        {
            get => _IsCustomResolution;
            set
            {
                _IsCustomResolution = value;
                RaisePropertyChanged(nameof(IsCustomResolution));
            }
        }

        public bool IsPatching
        {
            get => _IsPatching;
            set
            {
                _IsPatching = value;
                RaisePropertyChanged(nameof(IsPatching));

                // Update linked properties
                RaisePropertyChanged(nameof(IsResolutionChangeSupported));
                RaisePropertyChanged(nameof(IsLanguageChangeSupported));
                RaisePropertyChanged(nameof(IsCompatibilityFixSupported));
                RaisePropertyChanged(nameof(IsTexturePackSupported));

                if (!value)
                    CommandManager.InvalidateRequerySuggested();
            }
        }
        
        public string InformationText
        {
            get => _InformationText;
            set
            {
                _InformationText = value;
                RaisePropertyChanged(nameof(InformationText));
            }
        }
        public string StatusMessageText
        {
            get => _StatusMessageText;
            set
            {
                _StatusMessageText = value;
                RaisePropertyChanged(nameof(StatusMessageText));
            }
        }
        
        public string VersionInfoText => "Version: " + AssemblyInfo.VersionString;

        #region Member of IDataErrorInfo

        public string this[string columnName]
        {
            get
            {
                ValidationFailure failureObjectOrNull = _MainViewModelValidator.Validate(this).Errors
                                                                               .FirstOrDefault(error => error.PropertyName == columnName);

                return failureObjectOrNull != null ? failureObjectOrNull.ErrorMessage : NONE_ERROR;
            }
        }
        public string Error
        {
            get
            {
                ValidationResult result = _MainViewModelValidator.Validate(this);
                if (result.Errors.Any())
                {
                    string[] failureMessages = result.Errors.Select(failureObj => failureObj.ErrorMessage).ToArray();

                    return string.Join(Environment.NewLine, failureMessages);
                }

                return NONE_ERROR;
            }
        }
        #endregion
        
        private bool AnyValidationFailed => Error != NONE_ERROR;

        #region Commands

        public ICommand OpenExeFileDialogCommand { get; }
        public ICommand InstallDieNeueWeltCommand { get; }

        public ICommand ShowReadmeWindowCommand { get; }
        public ICommand ApplySettingsCommand { get; }
        public ICommand StartGameCommand { get; }
        #endregion

        private void HandleOpenExeFileDialog()
        {
            var fileDialog = new OpenFileDialog // TODO: FireEmerald: Move to service.
            {
                Title = @"Select S4.exe from '<InstallDir>\S4.exe' ...",
                Filter = "Settlers 4 Executable|S4.exe",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            bool? result = fileDialog.ShowDialog();
            if (result != null && result == true)
            {
                SettlersExePath = fileDialog.FileName;
            }
        }

        private void HandleInstallDieNeueWelt()
        {
            // TODO: FireEmerald: Add-ons can be only installed if the application is running as administrator: https://stackoverflow.com/questions/2818179/how-do-i-force-my-net-application-to-run-as-administrator

            InformationText = string.Empty;
            IsPatching = true;

            DriveInfo addonDisk = SearchForAddonDisk(GameAddon.DieNeueWelt);

            if (addonDisk == null)
            {
                LogAdapter.Log(LogLevel.Information, $"Please insert the disk for {GameAddon.DieNeueWelt}, none matching drive found.");
            }
            else
            {
                InstallAddon(GameAddon.DieNeueWelt, addonDisk);
                _Game.RefreshInstalledAddons();
            }

            IsPatching = false;

            // Update linked properties
            RaisePropertyChanged(nameof(AddonStatusDieNeueWelt));
        }

        private DriveInfo SearchForAddonDisk(GameAddon gameAddon)
        {
            LogAdapter.Log(LogLevel.Information, $"Searching for drive {gameAddon}...");

            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.CDRom && drive.IsReady)
                {
                    if (_Game.IsAddonDisk(GameAddon.DieNeueWelt, drive))
                    {
                        return drive;
                    }

                    LogAdapter.Log(LogLevel.Information, $"{drive.Name} not valid for {gameAddon}, skipped.");
                }
                else
                {
                    LogAdapter.Log(LogLevel.Information, $"{drive.Name} not of type {DriveType.CDRom} or not ready, skipped.");
                }
            }

            return null;
        }

        private void InstallAddon(GameAddon gameAddon, DriveInfo addonDisk)
        {
            LogAdapter.Log(LogLevel.Information, $"{gameAddon} found on {addonDisk.Name}, installing...");

            Result addonInstalled = _Game.InstallAddon(gameAddon, addonDisk);

            if (addonInstalled.Success)
            {
                LogAdapter.Log(LogLevel.Information, $"{gameAddon} successfully installed.");
            }
            else
            {
                LogAdapter.Log(LogLevel.Error, addonInstalled.ErrorMessage);
                LogAdapter.Log(LogLevel.Warning, $"Installation of {gameAddon} failed.");
            }
        }

        private void HandleShowReadmeWindow()
        {
            if (!_WindowManager.ReadmeWindowIsOpen)
                _WindowManager.OpenReadmeWindow();
        }

        private void HandleApplySettings()
        {
            if (_Game.IsRunning)
            {
                LogAdapter.Log(LogLevel.Information, "Game is running, close it and try it again.");
                return;
            }

            InformationText = string.Empty;
            IsPatching = true;

            GameModule supportedModules = _Game.SupportedModules;
            if (supportedModules.HasFlag(GameModule.CompatibilityFix))
            {
                if (ApplyCompatibilityFix)
                {
                    _Game.ApplyCompatibilityFix();
                }
                else
                {
                    _Game.RemoveCompatibilityFix();
                }
            }

            if (supportedModules.HasFlag(GameModule.TexturePack))
            {
                if (ActivateTexturePack)
                {
                    _Game.ActivateTropicalTextures();
                }
                else
                {
                    _Game.DeactivateTropicalTextures();
                }
            }

            if (supportedModules.HasFlag(GameModule.Resolution))
            {
                if (SelectedResolution == GameResolution.Custom)
                {
                    _Game.ChangeResolution(ushort.Parse(CustomWidth), ushort.Parse(CustomHeight));
                }
                else
                {
                    _Game.ChangeResolution(SelectedResolution);
                }
            }

            if (supportedModules.HasFlag(GameModule.Language))
            {
                _Game.ChangeLanguage(SelectedLanguage);
                // In this case NotifyUserAboutChangesWereApplied will be called later.
            }
            else
            {
                NotifyUserAboutChangesWereApplied(new Result());
            }
        }

        private void HandleLanguageChangeCompleted(Result result)
        {
            NotifyUserAboutChangesWereApplied(result);
        }

        private void NotifyUserAboutChangesWereApplied(Result result)
        {
            if (!result.Success)
            {
                LogAdapter.Log(LogLevel.Warning, result.ErrorMessage);
            }
            else
            {
                LogAdapter.Log(LogLevel.Information, "Changes successfully applied.");
            }

            IsPatching = false;
        }

        private void HandleStartGame()
        {
            if (_Game.IsRunning)
            {
                LogAdapter.Log(LogLevel.Information, "Game is already running.");
            }
            else
            {
                _Game.Start();
            }
        }
    }
}