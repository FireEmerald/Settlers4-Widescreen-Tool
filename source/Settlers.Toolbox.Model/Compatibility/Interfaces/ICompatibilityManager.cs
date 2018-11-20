using System.IO;

namespace Settlers.Toolbox.Model.Compatibility.Interfaces
{
    public interface ICompatibilityManager
    {
        bool IsFixApplied(DirectoryInfo installDir);

        void ApplyFix(DirectoryInfo installDir);
        void RemoveFix(DirectoryInfo installDir);
    }
}