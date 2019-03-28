using System;
using System.Collections.Generic;

namespace Settlers.Toolbox.Infrastructure.Cabinet.Interfaces
{
    public interface ICabContainer : IDisposable
    {
        int FileCount { get; }
        IReadOnlyList<CabFile> FileList { get; }
        
        bool SaveFile(CabFile file, string destinationPath);
    }
}