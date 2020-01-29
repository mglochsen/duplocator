using System.Collections.Generic;
using Duplocator.Data;
using Duplocator.Services;

namespace Duplocator.Duplocators
{
    public class FileSizeDuplocator : KeyCompareDuplocator
    {
        private readonly IFileService _fileService;

        public FileSizeDuplocator(IFileService fileService)
        {
            _fileService = fileService;
        }

        public IEnumerable<DuplicateGroup> GetDuplicates(IEnumerable<DuplicateGroup> duplicateGroups)
        {
            return GetDuplicates(duplicateGroups, _fileService.GetFileSize);
        }
    }
}