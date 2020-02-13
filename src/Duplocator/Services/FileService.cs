using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Duplocator.Services
{
    /// <summary>
    /// Implements a file service that uses the <see cref="System.IO" /> classes. 
    /// </summary>
    public class FileService : IFileService
    {
        /// <inheritdoc/>
        public IEnumerable<string> GetFilesInFolder(string path)
        {
            return Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories);
        }

        /// <inheritdoc/>
        public long GetFileSize(string filePath)
        {
            return new FileInfo(filePath).Length;
        }

        /// <inheritdoc/>
        public string GetFileHash(string filePath, uint? maxByteLength = null)
        {
            using (var sha = SHA256.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    if (maxByteLength.HasValue)
                    {
                        var bytes = new byte[maxByteLength.Value];
                        stream.Read(bytes, 0, bytes.Length);
                        return GetReadableHash(sha.ComputeHash(bytes));
                    }

                    return GetReadableHash(sha.ComputeHash(stream));
                }
            }
        }

        private static string GetReadableHash(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }
    }
}