using System.Collections.Generic;
using Duplocator.Data;
using Duplocator.Services;

namespace Duplocator.Duplocators
{
    public class HashDuplocator : KeyCompareDuplocator
    {
        private readonly IFileService _fileService;

        public HashDuplocator(IFileService fileService)
        {
            _fileService = fileService;
        }

        public IEnumerable<DuplicateGroup> GetDuplicates(IEnumerable<DuplicateGroup> duplicateGroups, uint? maxByteLength = null)
        {
            return GetDuplicates(duplicateGroups, filePath => _fileService.GetFileHash(filePath, maxByteLength));
        }
    }
}