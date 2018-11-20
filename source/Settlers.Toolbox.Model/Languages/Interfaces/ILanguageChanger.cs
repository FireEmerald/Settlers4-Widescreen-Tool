using System;
using System.IO;

using Settlers.Toolbox.Infrastructure;

namespace Settlers.Toolbox.Model.Languages.Interfaces
{
    public interface ILanguageChanger
    {
        event Action<Result> LanguageChangeCompleted;

        GameLanguage DetectLanguage(DirectoryInfo installDir);

        void ChangeLanguage(DirectoryInfo installDir, GameLanguage languageToInstall);
    }
}