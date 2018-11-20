using System.Threading.Tasks;

namespace Settlers.Toolbox.Model.Updates.Interfaces
{
    public interface IUpdateChecker
    {
        Task<VersionPackage> RetrieveLatestUpdateOrNull();
    }
}