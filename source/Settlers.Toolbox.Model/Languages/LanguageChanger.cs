using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Settlers.Toolbox.Infrastructure;
using Settlers.Toolbox.Infrastructure.ExtensionMethods;
using Settlers.Toolbox.Infrastructure.IO.Interfaces;
using Settlers.Toolbox.Infrastructure.Json.Interfaces;
using Settlers.Toolbox.Infrastructure.Logging;
using Settlers.Toolbox.Infrastructure.Network.Interfaces;
using Settlers.Toolbox.Infrastructure.Reporting.Interfaces;
using Settlers.Toolbox.Model.Languages.Interfaces;

namespace Settlers.Toolbox.Model.Languages
{
    public class LanguageChanger : ILanguageChanger
    {
        private const string JSON_LANGUAGE_DEFINITIONS_URL = @""; // TODO: FireEmerald: Add URL.

        private const string MISCDATA1_RELATIVE_PATH = @"Config\MiscData1.cfg";
        private const string MISCDATA1_SECTION = "MISCDATA1";
        private const string MISCDATA1_KEY_LANGUAGE = "Language";

        // TODO: FireEmerald: If the downloader would store this any downloaded file would be cached.
        private readonly IList<(GameLanguage language, FileInfo languagePackFile)> _DownloadedLanguagePacks;

        private readonly IReportManager _ReportManager;
        private readonly IStringDownloader _StringDownloader;
        private readonly IFileDownloader _FileDownloader;
        private readonly IJsonAdapter _JsonAdapter;
        private readonly IIniFileAdapter _IniFileAdapter;
        private readonly IZipFileAdapter _ZipFileAdapter;

        public event Action<Result> LanguageChangeCompleted;

        public LanguageChanger(IReportManager reportManager, IStringDownloader stringDownloader, IFileDownloader fileDownloader, IJsonAdapter jsonAdapter, IIniFileAdapter iniFileAdapter, IZipFileAdapter zipFileAdapter)
        {
            if (reportManager == null) throw new ArgumentNullException(nameof(reportManager));
            if (stringDownloader == null) throw new ArgumentNullException(nameof(stringDownloader));
            if (fileDownloader == null) throw new ArgumentNullException(nameof(fileDownloader));
            if (jsonAdapter == null) throw new ArgumentNullException(nameof(jsonAdapter));
            if (iniFileAdapter == null) throw new ArgumentNullException(nameof(iniFileAdapter));
            if (zipFileAdapter == null) throw new ArgumentNullException(nameof(zipFileAdapter));

            _ReportManager = reportManager;
            _StringDownloader = stringDownloader;
            _FileDownloader = fileDownloader;
            _JsonAdapter = jsonAdapter;
            _IniFileAdapter = iniFileAdapter;
            _ZipFileAdapter = zipFileAdapter;

            _DownloadedLanguagePacks = new List<(GameLanguage language, FileInfo languagePackFile)>();
        }

        ~LanguageChanger()
        {
            foreach ((GameLanguage language, FileInfo languagePackFile) downloadedLanguagePack in _DownloadedLanguagePacks)
            {
                downloadedLanguagePack.languagePackFile.Refresh();

                if (downloadedLanguagePack.languagePackFile.Exists)
                    downloadedLanguagePack.languagePackFile.Delete();
            }
        }

        public GameLanguage DetectLanguage(DirectoryInfo installDir)
        {
            if (installDir == null) throw new ArgumentNullException(nameof(installDir));

            FileInfo miscData1File = GetMiscData1File(installDir);

            string miscData1LanguageValue = _IniFileAdapter.ReadValueFromFile(MISCDATA1_SECTION, MISCDATA1_KEY_LANGUAGE, miscData1File.FullName);

            if (Enum.TryParse(miscData1LanguageValue, out GameLanguage gameLanguage))
            {
                return gameLanguage;
            }

            return GameLanguage.Unknown;
        }

        public void ChangeLanguage(DirectoryInfo installDir, GameLanguage languageToInstall)
        {
            if (installDir == null) throw new ArgumentNullException(nameof(installDir));

            ChangeTextLanguage(installDir, languageToInstall);

            RaiseLanguageChangeCompleted(new Result()); // TODO: FireEmerald: REMOVE TESTING CODE

            //Result voiceLanguageChanged = ChangeVoiceLanguage(installDir, languageToInstall);
            //if (!voiceLanguageChanged.Success)
            //    RaiseLanguageChangeCompleted(voiceLanguageChanged);
        }

        private void ChangeTextLanguage(DirectoryInfo installDir, GameLanguage languageToInstall)
        {
            FileInfo miscData1File = GetMiscData1File(installDir);

            int miscData1LanguageValue = GetLanguageId(languageToInstall);

            _IniFileAdapter.WriteValueToFile(MISCDATA1_SECTION, MISCDATA1_KEY_LANGUAGE, miscData1LanguageValue.ToString(), miscData1File.FullName);

            LogAdapter.Log(LogLevel.Information, "Text language changed ... OK");
        }

        private FileInfo GetMiscData1File(DirectoryInfo installDir)
        {
            var miscData1File = new FileInfo(Path.Combine(installDir.FullName, MISCDATA1_RELATIVE_PATH));

            if (!miscData1File.Exists)
            {
                throw new FileNotFoundException($"Unable to access '{MISCDATA1_RELATIVE_PATH}', file does not exist."); // TODO: FireEmerald: Replace
            }

            return miscData1File;
        }

        private Result ChangeVoiceLanguage(DirectoryInfo installDir, GameLanguage languageToInstall)
        {
            LogAdapter.Log(LogLevel.Information, "Changing audio and video files...");

            string languagesJson = _StringDownloader.DownloadString(JSON_LANGUAGE_DEFINITIONS_URL);
            if (string.IsNullOrEmpty(languagesJson))
            {
                return new Result("Download of sound pack links failed, skipped.");
            }

            var languageLinks = _JsonAdapter.DeserializeObject<LanguagePackLinks>(languagesJson);

            LanguagePackLink languagePackLink = GetLanguagePackLinkOrNull(languageToInstall, languageLinks);
            if (languagePackLink == null)
            {
                return new Result("None sound pack for selected language exists, skipped.");
            }

            FileInfo alreadyDownloadedLanguagePackFileOrNull = GetAlreadyDownloadedLanguagePackFileOrNull(languageToInstall, languagePackLink.Sha1);
            if (alreadyDownloadedLanguagePackFileOrNull != null)
            {
                InstallLanguagePack(installDir, alreadyDownloadedLanguagePackFileOrNull);
            }
            else
            {
                DownloadLanguagePack(installDir, languagePackLink);
            }

            return new Result();
        }


        private FileInfo GetAlreadyDownloadedLanguagePackFileOrNull(GameLanguage language, string sha1)
        {
            (GameLanguage, FileInfo languagePackFile) downloadedLanguagePack = _DownloadedLanguagePacks.SingleOrDefault(pack => pack.language == language);
            if (!downloadedLanguagePack.Equals(default(ValueTuple<GameLanguage, FileInfo>)))
            {
                downloadedLanguagePack.languagePackFile.Refresh();

                if (downloadedLanguagePack.languagePackFile.Exists && downloadedLanguagePack.languagePackFile.HashEquals(sha1))
                {
                    LogAdapter.Log(LogLevel.Information, "Download of sound pack skipped, already downloaded.");

                    return downloadedLanguagePack.languagePackFile;
                }

                if (downloadedLanguagePack.languagePackFile.Exists)
                    downloadedLanguagePack.languagePackFile.Delete();

                _DownloadedLanguagePacks.Remove(downloadedLanguagePack);
            }

            return null;
        }

        private void DownloadLanguagePack(DirectoryInfo installDir, LanguagePackLink languagePackLink)
        {
            //await Task.Delay(5000); // TODO: FireEmerald: REMOVE TEST CODE
            //LogAdapter.Log(LogLevel.Information, "DONE");
            //RaiseLanguageChangeCompleted(new Result());
            //return;

            var tempFile = new FileInfo(Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".tmp"));

            // Events
            _FileDownloader.DownloadProgressUpdate += (statusMessage) => _ReportManager.ReportStatus(statusMessage);
            _FileDownloader.DownloadFailed += (errorMessage) => HandleDownloadFailed(tempFile, errorMessage);
            _FileDownloader.DownloadSuccessful += () => HandleDownloadSuccessfule(installDir, tempFile, languagePackLink.Sha1, languagePackLink.Id);

            _FileDownloader.DownloadFile(languagePackLink.Url, tempFile);
        }

        private void HandleDownloadFailed(FileInfo languagePackFile, string errorMessage)
        {
            LogAdapter.Log(LogLevel.Error, errorMessage);
            RaiseLanguageChangeCompleted(new Result("Download of language pack ... FAILED"));

            languagePackFile.Refresh();

            if (languagePackFile.Exists)
                languagePackFile.Delete();
        }

        private void HandleDownloadSuccessfule(DirectoryInfo installDir, FileInfo languagePackFile, string sha1, int languageId)
        {
            LogAdapter.Log(LogLevel.Information, "Download of language pack ... OK");

            languagePackFile.Refresh();

            if (languagePackFile.Exists && languagePackFile.HashEquals(sha1))
            {
                LogAdapter.Log(LogLevel.Information, "Validating language pack ... OK");

                _DownloadedLanguagePacks.Add(((GameLanguage) languageId, languagePackFile));

                InstallLanguagePack(installDir, languagePackFile);
            }
            else
            {
                RaiseLanguageChangeCompleted(new Result("Validating language pack ... FAILED"));

                if (languagePackFile.Exists)
                    languagePackFile.Delete();
            }
        }

        private void InstallLanguagePack(DirectoryInfo installDir, FileInfo languagePackFile)
        {
            var tempDir = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));
            tempDir.Create();

            _ZipFileAdapter.ExtractToDirectory(languagePackFile, tempDir);

            CopyAndReplace(tempDir, installDir);
            tempDir.Delete(true);

            LogAdapter.Log(LogLevel.Information, "Applying language pack ... OK");
            RaiseLanguageChangeCompleted(new Result());
        }

        private void CopyAndReplace(DirectoryInfo sourceDir, DirectoryInfo targetDir)
        {
            foreach (FileInfo file in sourceDir.EnumerateFiles())
            {
                string targetFilePath = Path.Combine(targetDir.FullName, file.Name);
                file.CopyTo(targetFilePath, true);
            }

            foreach (DirectoryInfo directory in sourceDir.EnumerateDirectories())
            {
                DirectoryInfo createdDir = targetDir.CreateSubdirectory(directory.Name);
                CopyAndReplace(directory, createdDir);
            }
        }

        private int GetLanguageId(GameLanguage gameLanguage)
        {
            switch (gameLanguage)
            {
                case GameLanguage.Unknown:
                    throw new InvalidOperationException($"Enumeration type {gameLanguage} does not hold a valid language identifier.");
                default:
                    return (int) gameLanguage;
            }
        }

        private LanguagePackLink GetLanguagePackLinkOrNull(GameLanguage gameLanguage, LanguagePackLinks languagePackLinks)
        {
            int languageId = GetLanguageId(gameLanguage);

            return languagePackLinks.Languages.SingleOrDefault(x => x.Id == languageId);
        }

        private void RaiseLanguageChangeCompleted(Result result)
        {
            LanguageChangeCompleted?.Invoke(result);
        }
    }
}