using System.IO;
using Dfe.Rscd.Web.Application.Application.Services;
using Dfe.Rscd.Web.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Dfe.Rscd.Web.UnitTests.Application.Services
{
    public class AmendmentServiceTests
    {
        [Fact]
        public void UploadEvidence_ReturnsFolderName_GivenValidFile()
        {
            // Arrange
            var mockFileUploadService = new Mock<IFileUploadService>();
            var service = new EvidenceService(mockFileUploadService.Object);
            var file = new FormFile(new MemoryStream(), 0, 10, "test.txt", "test.txt")
            {
                Headers = new HeaderDictionary
                {
                    {"content-type", "text/plain"}
                }
            };

            // Act
            var folderName = service.UploadEvidence(new[] {file});

            // Assert
            Assert.True(!string.IsNullOrWhiteSpace(folderName));
        }

        [Fact]
        public void UploadEvidence_ReturnsNullFolderName_GivenEmptyFile()
        {
            // Arrange
            var mockFileUploadService = new Mock<IFileUploadService>();
            var service = new EvidenceService(mockFileUploadService.Object);

            // Act
            var folderName = service.UploadEvidence(new[] {new FormFile(new MemoryStream(), 0, 0, "", "")});

            // Assert
            Assert.Null(folderName);
        }
    }
}