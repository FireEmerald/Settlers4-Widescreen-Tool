using System;
using System.IO;

namespace Settlers.Toolbox.Infrastructure.ExtensionMethods
{
    public static class CompareExtensions
    {
        public static bool HashEquals(this FileInfo fileToHash, string expectedSha1)
        {
            string calculatedSha1 = fileToHash.CalculateSha1Hash();

            return expectedSha1.OrdinalEquals(calculatedSha1);
        }

        public static bool OrdinalEquals(this string a, string b)
        {
            return string.Equals(a, b, StringComparison.Ordinal);
        }
    }
}