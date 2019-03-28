using System.Collections.ObjectModel;
using System.Windows.Input;

using Settlers.Toolbox.Model.Languages;
using Settlers.Toolbox.Model.Resolutions;
using Settlers.Toolbox.ViewModels.Interfaces;

namespace Settlers.Toolbox.ViewModels
{
    public class DummyMainViewModel : IMainViewModel
    {
        public ObservableCollection<GameResolution> ResolutionCollection { get; } = new ObservableCollection<GameResolution>();
        public ObservableCollection<GameLanguage> LanguageCollection { get; } = new ObservableCollection<GameLanguage>();

        public GameResolution SelectedResolution { get; set; } = GameResolution.Default;
        public GameLanguage SelectedLanguage { get; set; } = GameLanguage.English;

        public string SettlersExePath { get; set; } = @"C:\Program Files\Settlers\S4.exe";

        public string CustomWidth { get; set; } = "1920";
        public string CustomHeight { get; set; } = "1080";

        public string AddonStatusDieNeueWelt { get; } = "Not installed";

        public bool IsResolutionChangeSupported { get; } = true;
        public bool IsLanguageChangeSupported { get; } = true;
        public bool IsCompatibilityFixSupported { get; } = true;
        public bool IsTexturePackSupported { get; } = true;

        public bool ApplyCompatibilityFix { get; set; } = false;
        public bool ActivateTexturePack { get; set; } = false;
        public bool IsCustomResolution { get; set; } = true;

        public bool IsPatching { get; } = false;

        public string InformationText { get; set; } = "Select your path, language, resolution and press 'Apply', then 'Start'.";
        public string StatusMessageText { get; set; } = "Ready.";

        public string VersionInfoText { get; } = "Version: X.X.X.X";

        public ICommand OpenExeFileDialogCommand { get; }
        public ICommand InstallDieNeueWeltCommand { get; }

        public ICommand ShowReadmeWindowCommand { get; }
        public ICommand ApplySettingsCommand { get; }
        public ICommand StartGameCommand { get; }
    }
}