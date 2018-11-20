using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Settlers.Toolbox.Infrastructure.ExtensionMethods
{
    public static class Sha1HashExtensions
    {
        public static string CalculateSha1Hash(this FileInfo file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            using (FileStream stream = file.OpenRead())
            {
                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    var hash = sha1.ComputeHash(stream);
                    var sb = new StringBuilder(hash.Length * 2);

                    foreach (byte b in hash)
                    {
                        sb.Append(b.ToString("X2")); // "x2" would produce a lowercase string.
                    }

                    return sb.ToString();
                }
            }
        }
    }
}