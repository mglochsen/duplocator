using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Duplocator
{
    public class FileService : IFileService
    {
        public IEnumerable<string> GetFilesInFolder(string path)
        {
            foreach (var filePath in Directory.GetDirectories(path).SelectMany(GetFilesInFolder))
            {
                yield return filePath;
            }

            foreach (var filePath in Directory.GetFiles(path))
            {
                yield return filePath;
            }
        }

        public long GetFileSize(string filePath)
        {
            return new FileInfo(filePath).Length;
        }

        public string GetFileHash(string filePath, uint? maxByteLength = null)
        {
            using (var sha1 = SHA1.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    if (maxByteLength.HasValue)
                    {
                        var bytes = new byte[maxByteLength.Value];
                        stream.Read(bytes, 0, bytes.Length);
                        return GetReadableHash(sha1.ComputeHash(bytes));
                    }

                    return GetReadableHash(sha1.ComputeHash(stream));
                }
            }
        }

        private static string GetReadableHash(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }
    }
}