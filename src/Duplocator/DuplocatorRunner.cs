using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Duplocator.Data;
using Duplocator.Duplocators;
using Duplocator.Services;

namespace Duplocator
{
    /// <summary>
    /// Finds duplicates using file size and hash checks.
    /// </summary>
    public class DuplocatorRunner
    {
        private readonly IFileSizeDuplocator _fileSizeDuplocator;
        private readonly IHashDuplocator _hashDuplocator;
        private readonly IFileService _fileService;

        public DuplocatorRunner()
        {
            _fileService = new FileService();
            _fileSizeDuplocator = new FileSizeDuplocator(_fileService);
            _hashDuplocator = new HashDuplocator(_fileService);
        }

        public DuplocatorRunner(IFileService fileService, IFileSizeDuplocator fileSizeDuplocator, IHashDuplocator hashDuplocator)
        {
            _fileService = fileService;
            _fileSizeDuplocator = fileSizeDuplocator;
            _hashDuplocator = hashDuplocator;
        }

        /// <summary>
        /// Get the duplicates using file size and hash checks.
        /// </summary>
        public RunnerResult GetDuplicates(RunnerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var stopWatch = Stopwatch.StartNew();

            var duplocatorFuncs = GetDuplocatorFuncs().ToArray();
            var initialGroup = GetInitialDuplicateGroup(options.FolderPath);
            var duplicateGroups = new [] { initialGroup };

            foreach (var duplocatorFunc in duplocatorFuncs)
            {
                duplicateGroups = duplicateGroups
                    .AsParallel()
                    .SelectMany(duplicateGroup => duplocatorFunc(duplicateGroup))
                    .ToArray();
            }

            return new RunnerResult(duplicateGroups, stopWatch.Elapsed);
        }

        private IEnumerable<Func<DuplicateGroup, IEnumerable<DuplicateGroup>>> GetDuplocatorFuncs()
        {
            yield return filePathGroups => _fileSizeDuplocator.GetDuplicates(filePathGroups);
            yield return filePathGroups => _hashDuplocator.GetDuplicates(filePathGroups, 1024);
            yield return filePathGroups => _hashDuplocator.GetDuplicates(filePathGroups);
        }

        private DuplicateGroup GetInitialDuplicateGroup(string folderPath)
        {
            var filePaths = _fileService.GetFilesInFolder(folderPath).ToArray();
            return new DuplicateGroup(filePaths);
        }
    }
}