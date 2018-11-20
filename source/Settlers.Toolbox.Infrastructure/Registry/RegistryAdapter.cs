using Settlers.Toolbox.Infrastructure.Registry.Interfaces;

namespace Settlers.Toolbox.Infrastructure.Registry
{
    public class RegistryAdapter : IRegistryAdapter
    {
        public IRegistryKeyAdapter LocalMachine()
        {
            return new RegistryKeyAdapter(Microsoft.Win32.Registry.LocalMachine);
        }
    }
}