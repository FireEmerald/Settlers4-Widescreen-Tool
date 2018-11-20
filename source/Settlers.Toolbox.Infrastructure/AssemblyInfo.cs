using System;
using System.Reflection;

namespace Settlers.Toolbox.Infrastructure
{
    public static class AssemblyInfo
    {
        public static string FullName => GetAssemblyName().FullName;

        public static string VersionString => Version.ToString();

        public static Version Version => GetAssemblyName().Version;


        private static AssemblyName GetAssemblyName()
        {
            return Assembly.GetExecutingAssembly().GetName();
        }
    }
}