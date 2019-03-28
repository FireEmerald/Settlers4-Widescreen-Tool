using System;
using System.Collections.Generic;
using System.IO;

using Settlers.Toolbox.Infrastructure;
using Settlers.Toolbox.Infrastructure.ExtensionMethods;
using Settlers.Toolbox.Infrastructure.Logging;
using Settlers.Toolbox.Model.Textures.Interfaces;

namespace Settlers.Toolbox.Model.Textures
{
    public class TextureSwapper : ITextureSwapper
    {
        private const string BACKUP_FILE_EXTENSION = ".bak";

        private readonly Dictionary<string, string> _FileMappings;

        public TextureSwapper()
        {
            _FileMappings = new Dictionary<string, string>
            {
                // Tropical    ,  Default
                { @"Gfx\41.gh5", @"Gfx\2.gh5" },
                { @"Gfx\41.gh6", @"Gfx\2.gh6" },
                { @"Gfx\41.gl5", @"Gfx\2.gl5" },
                { @"Gfx\41.gl6", @"Gfx\2.gl6" }
            };
        }

        public bool IsTropicalTexturePackActive(DirectoryInfo installDir)
        {
            if (installDir == null) throw new ArgumentNullException(nameof(installDir));

            foreach (KeyValuePair<string, string> fileMapping in _FileMappings)
            {
                FileInfo tropicalTextureFile = CombineToFileInfo(installDir, fileMapping.Key);
                FileInfo defaultTextureFile = CombineToFileInfo(installDir, fileMapping.Value);

                if (!tropicalTextureFile.Exists || !defaultTextureFile.Exists)
                    return false;

                if (!defaultTextureFile.HashEquals(tropicalTextureFile.CalculateSha1Hash()))
                    return false;
            }

            return true;
        }

        public void ActivateTropicalTextures(DirectoryInfo installDir)
        {
            if (installDir == null) throw new ArgumentNullException(nameof(installDir));

            Result texturesChanged = ChangeTextures(installDir, true);

            if (!texturesChanged.Success)
                LogAdapter.Log(LogLevel.Error, texturesChanged.ErrorMessage);

            LogAdapter.Log(LogLevel.Information, texturesChanged.Success
                ? "Texture pack activated ... OK"
                : "Texture pack activated ... FAILED");
        }

        public void DeactivateTropicalTextures(DirectoryInfo installDir)
        {
            if (installDir == null) throw new ArgumentNullException(nameof(installDir));

            Result texturesChanged = ChangeTextures(installDir, false);

            if (!texturesChanged.Success)
                LogAdapter.Log(LogLevel.Error, texturesChanged.ErrorMessage);

            LogAdapter.Log(LogLevel.Information, texturesChanged.Success
                ? "Texture pack deactivated ... OK"
                : "Texture pack deactivated ... FAILED");
        }

        private Result ChangeTextures(DirectoryInfo installDir, bool activateTropicalTextures)
        {
            foreach (KeyValuePair<string, string> fileMapping in _FileMappings)
            {
                FileInfo tropicalTextureFile = CombineToFileInfo(installDir, fileMapping.Key);
                FileInfo defaultTexturesFile = CombineToFileInfo(installDir, fileMapping.Value);

                FileInfo defaultBackupFile = CombineToFileInfo(installDir, fileMapping.Value + BACKUP_FILE_EXTENSION);

                if (activateTropicalTextures)
                {
                    if (!tropicalTextureFile.Exists)
                        return new Result($"Texture file {tropicalTextureFile.Name} from Add-on 'Die Neue Welt' does not exist.");

                    if (defaultBackupFile.Exists)
                        defaultBackupFile.Delete();

                    defaultTexturesFile.CopyTo(defaultBackupFile.FullName);
                    tropicalTextureFile.CopyTo(defaultTexturesFile.FullName, true);
                }
                else
                {
                    if (!defaultBackupFile.Exists)
                        return new Result($"Backup texture file {defaultBackupFile.FullName} does not exist.");

                    defaultTexturesFile.Delete();
                    defaultBackupFile.CopyTo(defaultTexturesFile.FullName);
                    defaultBackupFile.Delete();
                }
            }

            return new Result();
        }

        private FileInfo CombineToFileInfo(DirectoryInfo installDir, string relativeFilePath)
        {
            string fullFilePath = Path.Combine(installDir.FullName, relativeFilePath);

            return new FileInfo(fullFilePath);
        }
    }
}