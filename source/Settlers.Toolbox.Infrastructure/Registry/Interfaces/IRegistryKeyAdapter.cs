namespace Settlers.Toolbox.Infrastructure.Registry.Interfaces
{
    public interface IRegistryKeyAdapter
    {
        string Name { get; }

        IRegistryKeyAdapter OpenSubKey(string name, bool writable);

        IRegistryKeyAdapter CreateSubKey(string subkey);
        
        Result DeleteSubKeyTree(string subkey);

        object GetValue(string name, object defaultValue = null);

        void SetValue(string name, object value);
    }
}