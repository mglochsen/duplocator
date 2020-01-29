using System.Collections.Generic;
using Duplocator.Data;
using Duplocator.Services;

namespace Duplocator.Duplocators
{
    /// <summary>
    /// Get duplicates by elements file size.
    /// </summary>
    public class FileSizeDuplocator : KeyCompareDuplocator
    {
        private readonly IFileService _fileService;

        public FileSizeDuplocator(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Get duplicates by elements file size.
        /// </summary>
        /// <param name="duplicateGroups">The groups of duplicates to check.</param>
        /// <returns>The groups of duplicates that where found.</returns>
        public IEnumerable<DuplicateGroup> GetDuplicates(IEnumerable<DuplicateGroup> duplicateGroups)
        {
            return GetDuplicates(duplicateGroups, _fileService.GetFileSize);
        }
    }
}