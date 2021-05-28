using System;
using System.IO;

namespace AppReferences.Utilities
{
    class File
    {
        /// <summary>
        /// The method compares two files. It returns true if their checksums are equal, or else returns false.
        /// </summary>
        /// <param name="file1">string: first file</param>
        /// <param name="file2">string: second file</param>
        /// <returns>bool: true or false</returns>
        public bool Compare(string file1, string file2)
        {
            var crypto = new Crypto();
            byte[] checksum1 = crypto.CalculateChecksumWithMD5(file1);
            byte[] checksum2 = crypto.CalculateChecksumWithMD5(file2);

            return Convert.ToBase64String(checksum1) == Convert.ToBase64String(checksum2);
        }

        public static void IfDirDoesNotExistCreateIt(string path)
        {
            bool exists = Directory.Exists(path);

            if (!exists)
                Directory.CreateDirectory(path);
        }

        public static byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }

    }
}
