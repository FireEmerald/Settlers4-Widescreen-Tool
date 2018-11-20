using System;
using System.Runtime.InteropServices;

using Settlers.Toolbox.Infrastructure.IO.Interfaces;

namespace Settlers.Toolbox.Infrastructure.IO
{
    public class IniFileAdapter : IIniFileAdapter
    {
        // NOTE: FireEmerald: Both methods don't like special chars - so don't use them. Example: UTF-8 file containing 'ä' can't be read!
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileStringW(string lpAppName, string lpKeyName, string lpDefault, string lpReturnedString, int nSize, string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int WritePrivateProfileStringW(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        /// <summary>
        /// Retrieves a string from the specified section in an initialization file.
        /// </summary>
        /// <param name="section">The name of the section containing the key name.</param>
        /// <param name="key">The name of the key whose associated string is to be retrieved.</param>
        /// <param name="defaultValue">A default string. If the key cannot be found in the initialization file.</param>
        /// <param name="file">The name of the initialization file.</param>
        /// <returns>The return value is the number of characters copied to the buffer, not including the terminating null character.</returns>
        /// <remarks>The GetPrivateProfileString function searches the specified initialization file for a key that matches the name
        /// specified by the lpKeyName parameter under the section heading specified by the lpAppName parameter.
        /// If it finds the key, the function copies the corresponding string to the buffer. If the key does not exist,
        /// the function copies the default character string specified by the lpDefault parameter.</remarks>
        public string ReadValueFromFile(string section, string key, string file, string defaultValue = "")
        {
            if (string.IsNullOrEmpty(section)) throw new ArgumentNullException(nameof(section));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrEmpty(file)) throw new ArgumentNullException(nameof(file));
            if (defaultValue == null) throw new ArgumentNullException(nameof(defaultValue));

            var buffer = new string(' ', 1024);
            var length = GetPrivateProfileStringW(section, key, defaultValue, buffer, buffer.Length, file);

            return buffer.Substring(0, length);
        }

        /// <summary>
        /// Copies a string into the specified section of an initialization file.
        /// </summary>
        /// <param name="section">The name of the section to which the string will be copied. If the section does not exist, it is created.
        /// The name of the section is case-independent; the string can be any combination of uppercase and lowercase letters.</param>
        /// <param name="key">The name of the key to be associated with a string. If the key does not exist in the specified section, it is created.
        /// If this parameter is NULL, the entire section, including all entries within the section, is deleted.</param>
        /// <param name="value">A null-terminated string to be written to the file. If this parameter is NULL, the key pointed to by the key parameter is deleted.</param>
        /// <param name="file">The name of the initialization file.</param>
        /// <returns>If the function successfully copies the string to the initialization file, the return value is nonzero. If the function fails,
        /// or if it flushes the cached version of the most recently accessed initialization file, the return value is zero.</returns>
        public bool WriteValueToFile(string section, string key, string value, string file)
        {
            if (string.IsNullOrEmpty(section)) throw new ArgumentNullException(nameof(section));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(file)) throw new ArgumentNullException(nameof(file));

            return WritePrivateProfileStringW(section, key, " " + value, file) != 0;
        }
    }
}