using System.Security.Cryptography;

namespace AppReferences.Utilities
{
    /// <summary>
    /// This class contains security methods that are commonly used in the tech industry.
    /// </summary>
    public class Crypto
    {
        /// <summary>
        /// This method calculates the checksum of a file using MD5 cryptographic hash function.
        /// </summary>
        /// <param name="filename">string: File path/name</param>
        /// <returns>byte array: Crytographic hash value</returns>
        public byte[] CalculateChecksumWithMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = System.IO.File.OpenRead(filename))
                {
                    return md5.ComputeHash(stream);
                }
            }
        }
    }
}
