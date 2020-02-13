using System.Collections.Generic;

namespace Duplocator.Data
{
    /// <summary>
    /// A group of duplicates.
    /// </summary>
    public class DuplicateGroup
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DuplicateGroup" /> class.
        /// </summary>
        /// <param name="duplicates">The duplicates in the group.</param>
        public DuplicateGroup(string[] duplicates)
        {
            Duplicates = duplicates;
        }

        /// <summary>
        /// Gets the duplicates.
        /// </summary>
        public IReadOnlyCollection<string> Duplicates { get; }

        /// <summary>
        /// Gets the total number of duplicates in the group.
        /// </summary>
        /// <remarks>
        /// A total duplicate count of 1 means that this group only contain one element which then is unique.
        /// </remarks>
        public int TotalDuplicates => Duplicates.Count;

        /// <summary>
        /// Gets a value indicating if the group contains duplicates.
        /// </summary>
        /// <remarks>
        /// The group contains duplicates if their exist more than one item in the group.
        /// </remarks>
        public bool ContainsDuplicates => TotalDuplicates > 1;
    }
}