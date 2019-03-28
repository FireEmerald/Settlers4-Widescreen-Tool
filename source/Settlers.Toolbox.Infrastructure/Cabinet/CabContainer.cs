using System;
using System.Collections.Generic;
using System.IO;

using Settlers.Toolbox.Infrastructure.Cabinet.Interfaces;

using UnshieldSharp;

namespace Settlers.Toolbox.Infrastructure.Cabinet
{
    public class CabContainer : ICabContainer
    {
        private UnshieldCabinet _CabContainer;
        private IReadOnlyList<CabFile> _CachedFileList;

        public int FileCount => _CabContainer?.FileCount ?? -1;

        public CabContainer(FileInfo cabContainerFile)
        {
            if (cabContainerFile == null) throw new ArgumentNullException(nameof(cabContainerFile));
            if (!cabContainerFile.Exists) throw new FileNotFoundException(nameof(cabContainerFile));

            _CabContainer = UnshieldCabinet.Open(cabContainerFile.FullName);
        }

        public IReadOnlyList<CabFile> FileList
        {
            get
            {
                if (_CabContainer == null)
                    return null;

                if (_CachedFileList != null)
                    return _CachedFileList;

                var fileList = new List<CabFile>();
                for (int i = 0; i < FileCount; i++)
                {
                    string fileName = _CabContainer.FileName(i);

                    fileList.Add(new CabFile(i, fileName));
                }

                _CachedFileList = fileList;
                return fileList;
            }
        }

        public void Close()
        {
            _CabContainer = null;
        }

        public bool SaveFile(CabFile file, string destinationPath)
        {
            if (_CabContainer == null) throw new InvalidOperationException("Unable to save file, none cab file was loaded.");
            if (file.Index < 0 || file.Index > FileCount - 1) throw new IndexOutOfRangeException("Unable to save file, index of file is out of range.");

            return _CabContainer.FileSave(file.Index, destinationPath);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CabContainer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Release unmanaged and optionally managed resources.
        /// </summary>
        /// <param name="disposing">Releases managed resources, if true.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free managed resources here.
            }

            // Free unmanaged resources here.
            Close();
        }
    }
}