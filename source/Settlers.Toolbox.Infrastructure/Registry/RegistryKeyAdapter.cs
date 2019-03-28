using System;

using Microsoft.Win32;

using Settlers.Toolbox.Infrastructure.Registry.Interfaces;

namespace Settlers.Toolbox.Infrastructure.Registry
{
    public class RegistryKeyAdapter : IRegistryKeyAdapter
    {
        private readonly RegistryKey _RegistryKey;

        public string Name => _RegistryKey.Name;

        public RegistryKeyAdapter(RegistryKey registryKey)
        {
            if (registryKey == null) throw new ArgumentNullException(nameof(registryKey));

            _RegistryKey = registryKey;
        }

        public IRegistryKeyAdapter OpenSubKey(string name, bool writable)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            RegistryKey openedKey;

            try
            {
                openedKey = _RegistryKey.OpenSubKey(name, writable);
            }
            catch (Exception)
            {
                return null;
            }

            return openedKey != null ? new RegistryKeyAdapter(openedKey) : null;
        }

        public IRegistryKeyAdapter CreateSubKey(string subkey)
        {
            if (string.IsNullOrEmpty(subkey)) throw new ArgumentNullException(nameof(subkey));

            RegistryKey createdKey;

            try
            {
                createdKey = _RegistryKey.CreateSubKey(subkey);
            }
            catch (Exception)
            {
                return null;
            }

            return createdKey != null ? new RegistryKeyAdapter(createdKey) : null;

        }

        public Result DeleteSubKeyTree(string subkey)
        {
            if (string.IsNullOrEmpty(subkey)) throw new ArgumentNullException(nameof(subkey));

            if (OpenSubKey(subkey, true) != null)
            {
                _RegistryKey.DeleteSubKeyTree(subkey);
                return new Result();
            }

            return new Result($"Unable to delete sub key tree '{subkey}', unknown error.");
        }

        public object GetValue(string name, object defaultValue = null)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            object retrievedValue;

            try
            {
                retrievedValue = _RegistryKey.GetValue(name, defaultValue);
            }
            catch (Exception)
            {
                return defaultValue;
            }

            return retrievedValue;
        }

        public void SetValue(string name, object value)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (value == null) throw new ArgumentNullException(nameof(value));

            _RegistryKey.SetValue(name, value);
        }
    }
}