using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duplocator
{
    public abstract class KeyCompareDuplocator
    {
        protected IEnumerable<string[]> GetDuplicates<TKey>(IEnumerable<string[]> filePathGroups, Func<string, TKey> keyFunc)
        {
            return filePathGroups
                .AsParallel()
                .SelectMany(filePaths => FindDuplicates(filePaths, keyFunc))
                .Where(duplicates => duplicates.Length > 1)
                .AsEnumerable();
        }

        private IEnumerable<string[]> FindDuplicates<TKey>(string[] filePaths, Func<string, TKey> keyFunc)
        {
            var duplicateGroups = new ConcurrentDictionary<TKey, ConcurrentBag<string>>();
            var syncObject = new object();

            Parallel.ForEach(filePaths, filePath => {
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