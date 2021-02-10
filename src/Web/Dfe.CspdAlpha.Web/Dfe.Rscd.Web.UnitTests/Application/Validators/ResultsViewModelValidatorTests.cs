using System.Linq;
using Dfe.Rscd.Web.Application.Models.ViewModels.RemovePupil;
using Dfe.Rscd.Web.Application.Validators.RemovePupil;
using Xunit;

namespace Dfe.Rscd.Web.UnitTests.Application.Validators
{
    public class ResultsViewModelValidatorTests
    {
        [Fact]
        public void ResultsViewModel_IsValid_GivenSelectedID()
        {
            // Arrange
            var viewModel = new ResultsViewModel("ks4-june")
            {
                SelectedID = "12345"
            };

            var validator = new ResultsViewModelValidator();

            // Act
            var result = validator.Validate(viewModel);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ResultsViewModel_IsInvalid_WithoutSelectedID()
        {
            // Arrange
            var viewModel = new ResultsViewModel("ks4-june")
            {
                SelectedID = null
            };

            var validator = new ResultsViewModelValidator();

            // Act
            var result = validator.Validate(viewModel);

            // Assert
            Assert.False(result.IsValid);
            //Assert.Contains(result.Errors.First().ErrorMessage, "pupil");
        }

        [Fact]
        public void ResultsViewModel_IsInvalid_WithoutSelectedIDKS5()
        {
            // Arrange
            var viewModel = new ResultsViewModel("ks5")
            {
                SelectedID = null
            };

            var validator = new ResultsViewModelValidator();

            // Act
            var result = validator.Validate(viewModel);

            // Assert
            Assert.False(result.IsValid);
            //Assert.Contains(result.Errors.First().ErrorMessage, "student");
        }
    }
}
