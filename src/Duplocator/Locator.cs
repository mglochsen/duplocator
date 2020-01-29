using System;
using System.Collections.Generic;
using System.Linq;

namespace Duplocator
{
    public class Locator
    {
        private readonly FileSizeDuplocator _fileSizeDuplocator;
        private readonly HashDuplocator _hashDuplocator;
        private readonly IFileService _fileService;

        public Locator()
        {
            _fileService = new FileService();
            _fileSizeDuplocator = new FileSizeDuplocator(_fileService);
            _hashDuplocator = new HashDuplocator(_fileService);
        }

        public Locator(IFileService fileService, FileSizeDuplocator fileSizeDuplocator, HashDuplocator hashDuplocator)
        {
            _fileService = fileService;
            _fileSizeDuplocator = fileSizeDuplocator;
            _hashDuplocator = hashDuplocator;
        }

        public IEnumerable<string[]> GetDuplicates(string folderPath)
        {
            var filePaths = _fileService.GetFilesInFolder(folderPath).ToArray();
            var duplocatorFuncs = GetDuplocatorFuncs().ToArray();

            IEnumerable<string[]> filePathGroups = new [] { filePaths };

            foreach (var duplocatorFunc in duplocatorFuncs)
            {
                filePathGroups = duplocatorFunc(filePathGroups).ToArray();
            }

            return filePathGroups;
        }

        private IEnumerable<Func<IEnumerable<string[]>, IEnumerable<string[]>>> GetDuplocatorFuncs()
        {
            yield return filePathGroups => _fileSizeDuplocator.GetDuplicates(filePathGroups);
            yield return filePathGroups => _hashDuplocator.GetDuplicates(filePathGroups, 1024);
            yield return filePathGroups => _hashDuplocator.GetDuplicates(filePathGroups);
        }
    }
}