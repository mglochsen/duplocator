using System.Collections.Generic;

namespace Duplocator.Services
{
    /// <summary>
    /// Describes a service with file handling functions.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Gets the file paths of the files in a folder and its subfolders.
        /// </summary>
        IEnumerable<string> GetFilesInFolder(string path);

        /// <summary>
        /// Gets the size of of file in bytes.
        /// </summary>
        long GetFileSize(string filePath);

        /// <summary>
        /// Gets the hash of a file.
        /// If a max byte length is provided only the first bytes will be used to calculate the hash. This improves file read and calculation performance.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="maxByteLength"></param>
        /// <returns></returns>
        string GetFileHash(string filePath, uint? maxByteLength = null);
    }
}