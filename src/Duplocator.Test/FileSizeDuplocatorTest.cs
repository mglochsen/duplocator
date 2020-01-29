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
    public class FileSizeDuplocatorTest
    {
        [Fact]
        public void GetDuplicates_WithUniqueFileSizes_ReturnsNothing()
        {
            // Arrange
            var filePaths = CreateSampleFilePaths(10);
            var fileService = CreateFileServiceWithUniqueFileSizes();
            var target = CreateDuplocator(fileService);

            // Act
            var duplicates = target.GetDuplicates(new[] { new DuplicateGroup(filePaths) }).ToArray();

            // Assert
            duplicates.Should().BeEmpty();
        }

        [Fact]
        public void GetDuplicates_WithSameFileSizes_ReturnsWholeList()
        {
            // Arrange
            var filePaths = CreateSampleFilePaths(10);
            var fileService = CreateFileServiceWithConstantFileSizes();
            var target = CreateDuplocator(fileService);

            // Act
            var result = target.GetDuplicates(new[] { new DuplicateGroup(filePaths) }).ToArray();

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
            var fileService = CreateFileServiceWithFileSizesFromFileName();
            var target = CreateDuplocator(fileService);

            // Act
            var result = target.GetDuplicates(new[] { new DuplicateGroup(filePaths) }).ToArray();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(group => group.TotalDuplicates == 3 && group.Duplicates.Contains(filePaths[0]) && group.Duplicates.Contains(filePaths[1]) && group.Duplicates.Contains(filePaths[2]));
            result.Should().Contain(group => group.TotalDuplicates == 2 && group.Duplicates.Contains(filePaths[3]) && group.Duplicates.Contains(filePaths[4]));
        }

        private static FileSizeDuplocator CreateDuplocator(IFileService fileService)
        {
            return new FileSizeDuplocator(fileService);
        }

        private static IFileService CreateFileServiceWithUniqueFileSizes()
        {
            var count = 0;
            var syncObject = new object();

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock
                .Setup(service => service.GetFileSize(It.IsAny<string>()))
                .Returns(() =>
                {
                    lock (syncObject)
                    {
                        return ++count;
                    }
                });

            return fileServiceMock.Object;
        }

        private static IFileService CreateFileServiceWithConstantFileSizes()
        {
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock
                .Setup(service => service.GetFileSize(It.IsAny<string>()))
                .Returns(1);

            return fileServiceMock.Object;
        }

        private static IFileService CreateFileServiceWithFileSizesFromFileName()
        {
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock
                .Setup(service => service.GetFileSize(It.IsAny<string>()))
                .Returns<string>(filePath => long.Parse(Regex.Match(filePath, @"\d+").Value));

            return fileServiceMock.Object;
        }

        private static string[] CreateSampleFilePaths(int count)
        {
            return Enumerable.Range(0, count).Select(i => $"file{i}").ToArray();
        }
    }
}
