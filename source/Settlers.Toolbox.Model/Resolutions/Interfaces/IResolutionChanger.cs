using System.IO;

namespace Settlers.Toolbox.Model.Resolutions.Interfaces
{
    public interface IResolutionChanger
    {
        Resolution DetectResolution(DirectoryInfo installDir);

        void ChangeResolution(DirectoryInfo installDir, GameResolution gameResolution);
        void ChangeResolution(DirectoryInfo installDir, ushort width, ushort height);
    }
}