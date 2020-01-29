using System.Collections.Generic;

namespace Duplocator
{
    public class FileSizeDuplocator : KeyCompareDuplocator
    {
        private readonly IFileService _fileService;

        public FileSizeDuplocator(IFileService fileService)
        {
            _fileService = fileService;
        }

        public IEnumerable<string[]> GetDuplicates(IEnumerable<string[]> filePathGroups)
        {
            return GetDuplicates(filePathGroups, _fileService.GetFileSize);
        }
    }
}