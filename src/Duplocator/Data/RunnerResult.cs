using System.Collections.Generic;
using System.Linq;

namespace Duplocator.Data
{
    /// <summary>
    /// Result of the <see cref="DuplocatorRunner" />.
    /// </summary>
    public class RunnerResult
    {
        /// <summary>
        /// Creates a new instance of the <see cref="RunnerResult" /> class.
        /// </summary>
        /// <param name="duplicateGroups">The groups of duplicates.</param>
        public RunnerResult(DuplicateGroup[] duplicateGroups)
        {
            DuplicateGroups = duplicateGroups;
        }

        /// <summary>
        /// Gets the groups with their duplicates.
        /// </summary>
        public DuplicateGroup[] DuplicateGroups { get; }

        /// <summary>
        /// Gets the total number of duplicates in all groups.
        /// </summary>
        public int TotalDuplicates => DuplicateGroups.SelectMany(group => group.Duplicates).Count();

        /// <summary>
        /// Gets a value indicating at least one group contains duplicates.
        /// </summary>
        public bool ContainsDuplicates => DuplicateGroups.Any(group => group.ContainsDuplicates);
    }
}