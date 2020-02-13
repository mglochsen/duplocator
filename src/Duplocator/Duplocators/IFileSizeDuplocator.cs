using System.Collections.Generic;
using Duplocator.Data;

namespace Duplocator.Duplocators
{
    /// <summary>
    /// Get duplicates by elements file size.
    /// </summary>
    public interface IFileSizeDuplocator
    {
        /// <summary>
        /// Get duplicates by elements file size.
        /// </summary>
        /// <param name="duplicateGroup">The group of elements to check.</param>
        /// <returns>The groups of duplicates that where found.</returns>
        IEnumerable<DuplicateGroup> GetDuplicates(DuplicateGroup duplicateGroup);
    }
}
