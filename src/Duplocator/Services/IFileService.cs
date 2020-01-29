using System.Collections.Generic;

namespace Duplocator.Services
{
    public interface IFileService
    {
        IEnumerable<string> GetFilesInFolder(string path);

        long GetFileSize(string filePath);

        string GetFileHash(string filePath, uint? maxByteLength = null);
    }
}