using System.Linq;
using System.Text.RegularExpressions;
using Duplocator.Data;
using Duplocator.Duplocators;
using Duplocator.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace Duplocator.Test
{
    public class HashDuplocatorTest
    {
        [Fact]
        public void GetDuplicates_UsesOptions()
        {
            // Arrange
            const string filePath = "test";
            const uint maxByteLength = 1234;
            var filePaths = new[] { filePath };
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(service => service.GetFileHash(It.IsAny<string>(), It.IsAny<uint?>())).Returns("hash");

            var target = CreateDuplocator(fileServiceMock.Object);

            // Act
            target.GetDuplicates(new DuplicateGroup(filePaths), maxByteLength);

            // Assert
            fileServiceMock.Verify(service => service.GetFileHash(filePath, maxByteLength), Times.Once());
        }

        [Fact]
        public void GetDuplicates_WithUniqueFileSizes_ReturnsNothing()
        {
            // Arrange
            var filePaths = CreateSampleFilePaths(10);
            var fileService = CreateFileServiceWithUniqueHashes();
            var target = CreateDuplocator(fileService);

            // Act
            var duplicates = target.GetDuplicates(new DuplicateGroup(filePaths)).ToArray();

            // Assert
            duplicates.Should().BeEmpty();
        }

        [Fact]
        public void GetDuplicates_WithSameFileSizes_ReturnsWholeList()
        {
            // Arrange
            var filePaths = CreateSampleFilePaths(10);
            var fileService = CreateFileServiceWithConstantHashes();
            var target = CreateDuplocator(fileService);

            // Act
            var result = target.GetDuplicates(new DuplicateGroup(filePaths)).ToArray();

            // Assert
            result.Should().HaveCount(1);
            result[0].TotalDuplicates.Should().Be(filePaths.Length);
        }

        [Fact]
        public void GetDuplicates_WithDuplicates_ReturnsDuplicates()
        {
            // Arrange
            var filePaths = new[]
            {
                "firstfile1",
                "secondfile1",
                "thirdfile1",
                "firstfile2",
                "secondfile2",
                "firstfile3",
            };
            var fileService = CreateFileServiceWithHashesFromFileName();
            var target = CreateDuplocator(fileService);

            // Act
            var result = target.GetDuplicates(new DuplicateGroup(filePaths)).ToArray();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(group => group.TotalDuplicates == 3 && group.Duplicates.Contains(filePaths[0]) && group.Duplicates.Contains(filePaths[1]) && group.Duplicates.Contains(filePaths[2]));
            result.Should().Contain(group => group.TotalDuplicates == 2 && group.Duplicates.Contains(filePaths[3]) && group.Duplicates.Contains(filePaths[4]));
        }

        private static HashDuplocator CreateDuplocator(IFileService fileService)
        {
            return new HashDuplocator(fileService);
        }

        private static IFileService CreateFileServiceWithUniqueHashes()
        {
            var count = 0;
            var syncObject = new object();

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock
                .Setup(service => service.GetFileHash(It.IsAny<string>(), It.IsAny<uint?>()))
                .Returns(() =>
                {
                    lock (syncObject)
                    {
                        return $"hash_{++count}";
                    }
                });

            return fileServiceMock.Object;
        }

        private static IFileService CreateFileServiceWithConstantHashes()
        {
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock
                .Setup(service => service.GetFileHash(It.IsAny<string>(), It.IsAny<uint?>()))
                .Returns("hash_const");

            return fileServiceMock.Object;
        }

        private static IFileService CreateFileServiceWithHashesFromFileName()
        {
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock
                .Setup(service => service.GetFileHash(It.IsAny<string>(), It.IsAny<uint?>()))
                .Returns<string, uint?>((filePath, bytesLength) => $"hash_{Regex.Match(filePath, @"\d+").Value}");

            return fileServiceMock.Object;
        }

        private static string[] CreateSampleFilePaths(int count)
        {
            return Enumerable.Range(0, count).Select(i => $"file{i}").ToArray();
        }
    }
}
