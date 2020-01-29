using System.Collections.Generic;
using System.Linq;

namespace Duplocator.Data
{
    public class RunnerResult
    {
        public RunnerResult(DuplicateGroup[] duplicateGroups)
        {
            DuplicateGroups = duplicateGroups;
        }

        public DuplicateGroup[] DuplicateGroups { get; }

        public int TotalDuplicates => DuplicateGroups.SelectMany(group => group.Duplicates).Count();

        public bool ContainsDuplicates => DuplicateGroups.SelectMany(group => group.Duplicates).Any();
    }
}