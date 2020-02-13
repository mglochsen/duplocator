using Duplocator.Data;
using Duplocator.Duplocators;
using Duplocator.Services;
using Moq;
using Xunit;

namespace Duplocator.Test
{
    public class DuplocatorRunnerTest
    {
        [Fact]
        public void GetDuplicates_GetsFilesInFolder()
        {
            // Arrange
            const string folderPath = "fake_path";
            var options = new RunnerOptions(folderPath);
            var fileServiceMock = new Mock<IFileService>();
            var target = CreateRunner(fileServiceMock.Object);

            // Act
            target.GetDuplicates(options);

            // Assert
            fileServiceMock.Verify(service => service.GetFilesInFolder(folderPath), Times.Once);
        }

        [Fact]
        public void GetDuplicates_UsesExpectedDuplocators()
        {
            // Arrange
            const string folderPath = "fake_path";
            const uint expectedMaxByteLength = 1024;
            var options = new RunnerOptions(folderPath);
            var fileSizeDuplocatorMock = new Mock<IFileSizeDuplocator>();
            fileSizeDuplocatorMock
                .Setup(duplocator => duplocator.GetDuplicates(It.IsAny<DuplicateGroup>()))
                .Returns<DuplicateGroup>(group => new [] { group });
            var hashDuplocatorMock = new Mock<IHashDuplocator>();
            hashDuplocatorMock
                .Setup(duplocator => duplocator.GetDuplicates(It.IsAny<DuplicateGroup>(), It.IsAny<uint?>()))
                .Returns<DuplicateGroup, uint?>((group, maxByteLength) => new [] { group });
            var target = CreateRunner(fileSizeDuplocator: fileSizeDuplocatorMock.Object, hashDuplocator: hashDuplocatorMock.Object);

            // Act
            target.GetDuplicates(options);

            // Assert
            fileSizeDuplocatorMock.Verify(duplocator => duplocator.GetDuplicates(It.IsAny<DuplicateGroup>()), Times.Once());
            hashDuplocatorMock.Verify(duplocator => duplocator.GetDuplicates(It.IsAny<DuplicateGroup>(), expectedMaxByteLength), Times.Once());
            hashDuplocatorMock.Verify(duplocator => duplocator.GetDuplicates(It.IsAny<DuplicateGroup>(), null), Times.Once());
        }

        private static DuplocatorRunner CreateRunner(
            IFileService fileService = null,
            IFileSizeDuplocator fileSizeDuplocator = null,
            IHashDuplocator hashDuplocator = null)
        {
            return new DuplocatorRunner(
                fileService ?? new Mock<IFileService>().Object,
                fileSizeDuplocator ?? new Mock<IFileSizeDuplocator>().Object,
                hashDuplocator ?? new Mock<IHashDuplocator>().Object);
        }
    }
}
