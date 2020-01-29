using System.Collections.Generic;

namespace Duplocator
{
    public class HashDuplocator : KeyCompareDuplocator
    {
        private readonly IFileService _fileService;

        public HashDuplocator(IFileService fileService)
        {
            _fileService = fileService;
        }

        public IEnumerable<string[]> GetDuplicates(IEnumerable<string[]> filePathGroups, uint? maxByteLength = null)
        {
            return GetDuplicates(filePathGroups, filePath => _fileService.GetFileHash(filePath, maxByteLength));
        }
    }
}