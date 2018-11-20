using System.IO;

using Settlers.Toolbox.Infrastructure;

namespace Settlers.Toolbox.Model.Textures.Interfaces
{
    public interface ITextureSwapper
    {
        bool IsTropicalTexturePackActive(DirectoryInfo installDir);

        void ActivateTropicalTextures(DirectoryInfo installDir);
        void DeactivateTropicalTextures(DirectoryInfo installDir);
    }
}