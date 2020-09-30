using Dfe.CspdAlpha.Web.Application.Application.Services;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using Dfe.CspdAlpha.Web.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Dfe.Rscd.Web.UnitTests.Application
{
    public class AmendmentServiceTests
    {
        [Fact]
        public void UploadEvidence_ReturnsFolderName_GivenValidFile()
        {
            // Arrange
            var mockDomainAmendmentService = new Mock<IAmendmentService>();
            var mockFileUploadService = new Mock<IFileUploadService>();
            var service = new AmendmentService(mockDomainAmendmentService.Object, mockFileUploadService.Object);
            var file = new FormFile(new System.IO.MemoryStream(), 0, 10, "test.txt", "test.txt")
            {
                Headers = new HeaderDictionary
                {
                    { "content-type", "text/plain" }
                }
            };

            // Act
            var folderName = service.UploadEvidence(new[] { file });

            // Assert
            Assert.True(!string.IsNullOrWhiteSpace(folderName));
        }

        [Fact]
        public void UploadEvidence_ReturnsNullFolderName_GivenEmptyFile()
        {
            // Arrange
            var mockDomainAmendmentService = new Mock<IAmendmentService>();
            var mockFileUploadService = new Mock<IFileUploadService>();
            var service = new AmendmentService(mockDomainAmendmentService.Object, mockFileUploadService.Object);

            // Act
            var folderName = service.UploadEvidence(new[] { new FormFile(new System.IO.MemoryStream(), 0, 0, "", "") });

            // Assert
            Assert.Null(folderName);
        }
    }
}
