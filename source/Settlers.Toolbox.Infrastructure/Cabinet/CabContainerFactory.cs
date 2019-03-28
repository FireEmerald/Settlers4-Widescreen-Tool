using System;
using System.IO;

using Settlers.Toolbox.Infrastructure.Cabinet.Interfaces;

namespace Settlers.Toolbox.Infrastructure.Cabinet
{
    public class CabContainerFactory : ICabContainerFactory
    {
        public ICabContainer Create(FileInfo cabContainerFile)
        {
            if (cabContainerFile == null) throw new ArgumentNullException(nameof(cabContainerFile));
            
            return new CabContainer(cabContainerFile);
        }
    }
}