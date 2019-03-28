using System.Collections.ObjectModel;
using System.Windows.Input;

using Settlers.Toolbox.Model.Languages;
using Settlers.Toolbox.Model.Resolutions;

namespace Settlers.Toolbox.ViewModels.Interfaces
{
    public interface IMainViewModel
    {
        ObservableCollection<GameResolution> ResolutionCollection { get; }
        ObservableCollection<GameLanguage> LanguageCollection { get; }

        GameResolution SelectedResolution { get; set; }
        GameLanguage SelectedLanguage { get; set; }

        string SettlersExePath { get; set; }

        string CustomWidth { get; set; }
        string CustomHeight { get; set; }

        string AddonStatusDieNeueWelt { get; }

        bool IsResolutionChangeSupported { get; }
        bool IsLanguageChangeSupported { get; }
        bool IsCompatibilityFixSupported { get; }
        bool IsTexturePackSupported { get; }

        bool ApplyCompatibilityFix { get; set; }
        bool ActivateTexturePack { get; set; }
        bool IsCustomResolution { get; set; }

        bool IsPatching { get; }

        string InformationText { get; set; }
        string StatusMessageText { get; set; }

        string VersionInfoText { get; }

        ICommand OpenExeFileDialogCommand { get; }
        ICommand InstallDieNeueWeltCommand { get; }

        ICommand ShowReadmeWindowCommand { get; }
        ICommand ApplySettingsCommand { get; }
        ICommand StartGameCommand { get; }
    }
}