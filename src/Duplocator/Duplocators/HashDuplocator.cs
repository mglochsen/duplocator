using System;
using System.Collections.Generic;
using Duplocator.Data;
using Duplocator.Services;

namespace Duplocator.Duplocators
{
    /// <summary>
    /// Get duplicates by elements file hash.
    /// </summary>
    public class HashDuplocator : KeyCompareDuplocator, IHashDuplocator
    {
        private readonly IFileService _fileService;

        public HashDuplocator(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Get duplicates by elements file hash.
        /// </summary>
        /// <param name="duplicateGroups">The groups of duplicates to check.</param>
        /// <param name="maxByteLength">An optional value. If it is provided only the first bytes will be used to calculate the hash</param>
        /// <returns>The groups of duplicates that where found.</returns>
        [Obsolete("Use method for single group instead.")]
        public IEnumerable<DuplicateGroup> GetDuplicates(IEnumerable<DuplicateGroup> duplicateGroups, uint? maxByteLength = null)
        {
            return GetDuplicates(duplicateGroups, filePath => _fileService.GetFileHash(filePath, maxByteLength));
        }

        /// <inheritdoc />
        public IEnumerable<DuplicateGroup> GetDuplicates(DuplicateGroup duplicateGroup, uint? maxByteLength = null)
        {
            return GetDuplicates(duplicateGroup, filePath => _fileService.GetFileHash(filePath, maxByteLength));
        }
    }
}