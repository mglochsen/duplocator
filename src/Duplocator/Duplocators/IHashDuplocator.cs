using System.Collections.Generic;
using Duplocator.Data;

namespace Duplocator.Duplocators
{
    /// <summary>
    /// Get duplicates by elements file hash.
    /// </summary>
    public interface IHashDuplocator
    {
        /// <summary>
        /// Get duplicates by elements file hash.
        /// </summary>
        /// <param name="duplicateGroup">The group of elements to check.</param>
        /// <param name="maxByteLength">An optional value. If it is provided only the first bytes will be used to calculate the hash</param>
        /// <returns>The groups of duplicates that where found.</returns>
        IEnumerable<DuplicateGroup> GetDuplicates(DuplicateGroup duplicateGroup, uint? maxByteLength = null);
    }
}
