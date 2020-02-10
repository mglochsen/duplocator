using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duplocator.Data;

namespace Duplocator.Duplocators
{
    public abstract class KeyCompareDuplocator
    {
        [Obsolete("Use method for single group instead.")]
        protected IEnumerable<DuplicateGroup> GetDuplicates<TKey>(IEnumerable<DuplicateGroup> duplicateGroups, Func<string, TKey> keyFunc)
        {
            return duplicateGroups
                .AsParallel()
                .SelectMany(group => GetDuplicates(group, keyFunc));
        }

        protected IEnumerable<DuplicateGroup> GetDuplicates<TKey>(DuplicateGroup duplicateGroup, Func<string, TKey> keyFunc)
        {
            return FindDuplicates(duplicateGroup.Duplicates, keyFunc)
                .Select(duplicates => new DuplicateGroup(duplicates))
                .Where(group => group.ContainsDuplicates)
                .AsEnumerable();
        }

        private IEnumerable<string[]> FindDuplicates<TKey>(string[] filePaths, Func<string, TKey> keyFunc)
        {
            var duplicateGroups = new ConcurrentDictionary<TKey, ConcurrentBag<string>>();
            var syncObject = new object();

            Parallel.ForEach(filePaths, filePath =>
            {
                var key = keyFunc(filePath);

                duplicateGroups.AddOrUpdate(
                    key,
                    new ConcurrentBag<string> { filePath },
                    (k, list) =>
                    {
                        lock (syncObject)
                        {
                            list.Add(filePath);
                            return list;
                        }
                    });
            });

            return duplicateGroups.Values.Select(bag => bag.ToArray());
        }
    }
}