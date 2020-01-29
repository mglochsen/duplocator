namespace Duplocator.Data
{
    public class DuplicateGroup
    {
        public DuplicateGroup(string[] duplicates)
        {
            Duplicates = duplicates;
        }

        public string[] Duplicates { get; }

        public int TotalDuplicates => Duplicates.Length;

        public bool ContainsDuplicates => TotalDuplicates > 1;
    }
}