using System.IO;

namespace Settlers.Toolbox.Infrastructure.Cabinet.Interfaces
{
    public interface ICabContainerFactory
    {
        ICabContainer Create(FileInfo cabContainerFile);
    }
}