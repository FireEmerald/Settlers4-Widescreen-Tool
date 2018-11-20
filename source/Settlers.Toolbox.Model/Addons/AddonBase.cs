using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Settlers.Toolbox.Infrastructure;
using Settlers.Toolbox.Infrastructure.Cabinet;
using Settlers.Toolbox.Infrastructure.Cabinet.Interfaces;
using Settlers.Toolbox.Infrastructure.ExtensionMethods;
using Settlers.Toolbox.Model.Addons.Interfaces;

namespace Settlers.Toolbox.Model.Addons
{
    public abstract class AddonBase : IAddon
    {
        private const string DISK_MAIN_DIRECTORY_PATH = "S4";
        private const string DISK_DATA1_CAB_FILE_NAME = "data1.cab";

        private readonly ICabContainerFactory _CabContainerFactory;

        protected AddonBase(ICabContainerFactory cabContainerFactory)
        {
            if (cabContainerFactory == null) throw new ArgumentNullException(nameof(cabContainerFactory));

            _CabContainerFactory = cabContainerFactory;
        }

        public abstract GameAddon Id { get; }

        public abstract IDictionary<string, string> FileMappings { get; }

        public bool IsInstalled(DirectoryInfo installDir)
        {
            if (installDir == null) throw new ArgumentNullException(nameof(installDir));

            if (!UnlockRegistryKeysExist())
                return false;

            foreach (KeyValuePair<string, string> fileMapping in FileMappings)
            {
                string fullFilePath = Path.Combine(installDir.FullName, fileMapping.Value);

                if (!File.Exists(fullFilePath))
                    return false;
            }

            return true;
        }

        public bool IsAddonDisk(DriveInfo driveInfo)
        {
            if (driveInfo == null) throw new ArgumentNullException(nameof(driveInfo));

            DirectoryInfo diskMainDirectory = GetDiskMainDirectory(driveInfo);
            if (!diskMainDirectory.Exists)
                return false;

            FileInfo cabContainerFile = GetCabContainerFile(diskMainDirectory);
            if (!cabContainerFile.Exists)
                return false;

            IReadOnlyList<FileInfo> filesInMainDirOnDisk = diskMainDirectory.GetFiles("*", SearchOption.AllDirectories);
            IReadOnlyList<CabFile> filesInCabFileOnDisk = GetFileListFromCabContainerFile(cabContainerFile);

            foreach (KeyValuePair<string, string> fileMapping in FileMappings)
            {
                if (!filesInMainDirOnDisk.Any(file => file.Name.OrdinalEquals(fileMapping.Key)) &&
                    !filesInCabFileOnDisk.Any(cabFile => cabFile.Name.OrdinalEquals(fileMapping.Key)))
                    return false;
            }

            return true;
        }

        private DirectoryInfo GetDiskMainDirectory(DriveInfo driveInfo)
        {
            string diskMainDirectoryPath = Path.Combine(driveInfo.RootDirectory.FullName, DISK_MAIN_DIRECTORY_PATH);

            return new DirectoryInfo(diskMainDirectoryPath);
        }

        private FileInfo GetCabContainerFile(DirectoryInfo diskMainDirectory)
        {
            string cabContainerFilePath = Path.Combine(diskMainDirectory.FullName, DISK_DATA1_CAB_FILE_NAME);

            return new FileInfo(cabContainerFilePath);
        }

        private IReadOnlyList<CabFile> GetFileListFromCabContainerFile(FileInfo cabContainerFile)
        {
            using (ICabContainer cabContainer = _CabContainerFactory.Create(cabContainerFile))
            {
                return cabContainer.FileList;
            }
        }

        public Result InstallFromDisk(DirectoryInfo installDir, DriveInfo addonDisk)
        {
            if (installDir == null) throw new ArgumentNullException(nameof(installDir));
            if (addonDisk == null) throw new ArgumentNullException(nameof(addonDisk));

            DirectoryInfo diskMainDirectory = GetDiskMainDirectory(addonDisk);
            FileInfo cabContainerFile = GetCabContainerFile(diskMainDirectory);

            IReadOnlyList<FileInfo> filesInMainDirOnDisk = diskMainDirectory.GetFiles("*", SearchOption.AllDirectories);

            Result addonFilesCopied;
            using (ICabContainer cabContainer = _CabContainerFactory.Create(cabContainerFile))
            {
                addonFilesCopied = CopyAddonFilesToInstallDir(installDir, filesInMainDirOnDisk, cabContainer);
            }

            Result addonInRegistryUnlocked = CreateUnlockRegistryKeys();

            if (addonFilesCopied.Success)
            {
                if (addonInRegistryUnlocked.Success)
                    return new Result();
            }

            // Revert partial changes
            RemoveAddonFilesFromInstallDir(installDir);

            return addonFilesCopied.Success ? addonInRegistryUnlocked : addonFilesCopied;
        }

        private Result CopyAddonFilesToInstallDir(DirectoryInfo installDir, IReadOnlyList<FileInfo> filesInMainDirOnDisk, ICabContainer cabContainer)
        {
            foreach (KeyValuePair<string, string> fileMapping in FileMappings)
            {
                string fullDestinationFilePath = Path.Combine(installDir.FullName, fileMapping.Value);

                if (File.Exists(fullDestinationFilePath))
                    RemoveReadOnlyFileAttribute(fullDestinationFilePath);

                FileInfo matchingMainDirFile = filesInMainDirOnDisk.FirstOrDefault(file => file.Name.OrdinalEquals(fileMapping.Key));
                if (matchingMainDirFile != null)
                {
                    matchingMainDirFile.CopyTo(fullDestinationFilePath, true); // TODO: FireEmerald: Crashes if the dir does not exist.
                    continue;
                }

                CabFile matchingCabFile = cabContainer.FileList.FirstOrDefault(cabFile => cabFile.Name.OrdinalEquals(fileMapping.Key));
                if (matchingCabFile != null)
                {
                    if (cabContainer.SaveFile(matchingCabFile, fullDestinationFilePath))
                        continue;

                    return new Result($"Unable to copy file '{matchingCabFile.Name}' from cab container to install directory, unknown error.");
                }

                return new Result($"Unable to copy file '{fileMapping.Key}' from disk to install directory, source file not found.");
            }

            return new Result();
        }

        private void RemoveReadOnlyFileAttribute(string fullFilePath)
        {
            FileAttributes attributes = File.GetAttributes(fullFilePath);

            if (attributes.HasFlag(FileAttributes.ReadOnly))
                File.SetAttributes(fullFilePath, attributes &~ FileAttributes.ReadOnly);
        }

        private void RemoveAddonFilesFromInstallDir(DirectoryInfo installDir)
        {
            foreach (KeyValuePair<string, string> fileMapping in FileMappings)
            {
                string fullFilePath = Path.Combine(installDir.FullName, fileMapping.Value);

                if (File.Exists(fullFilePath))
                {
                    RemoveReadOnlyFileAttribute(fullFilePath);
                    File.Delete(fullFilePath);
                }
            }
        }

        protected abstract bool UnlockRegistryKeysExist();
        protected abstract Result CreateUnlockRegistryKeys();
    }
}