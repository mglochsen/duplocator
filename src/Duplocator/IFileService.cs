using System.Collections.Generic;

namespace Duplocator
{
    public interface IFileService
    {
         IEnumerable<string> GetFilesInFolder(string path);

         long GetFileSize(string filePath);

         string GetFileHash(string filePath, uint? maxByteLength = null);
    }
}