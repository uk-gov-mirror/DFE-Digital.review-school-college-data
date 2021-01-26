using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using Dfe.CspdAlpha.Web.Application.Validators.RemovePupil;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Dfe.Rscd.Web.UnitTests.Application.Validators
{
    public class SearchPupilsViewModelValidatorTests
    {
        [Fact]
        public void SearchPupilsViewModel_IsValid_GivenSearchTypeIsPupilIDAndValidPupilID()
        {
            // Arrange
            var viewModel = new SearchPupilsViewModel("ks5")
            {
                SearchType = QueryType.PupilID,
                PupilID = "79"
            };

            var validator = new SearchPupilsViewModelValidator();

            // Act
            var result = validator.Validate(viewModel);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void SearchPupilsViewModel_IsInvalid_GivenSearchTypeIsPupilIDAndNoPupilID()
        {
            // Arrange
            var viewModel = new SearchPupilsViewModel("ks5")
            {
                SearchType = QueryType.PupilID,
                PupilID = " "
            };

            var validator = new SearchPupilsViewModelValidator();

            // Act
            var result = validator.Validate(viewModel);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void SearchPupilsViewModel_IsInvalid_GivenSearchTypeIsPupilIDAndInvalidULN()
        {
            // Arrange
            var viewModel = new SearchPupilsViewModel("ks5")
            {
                SearchType = QueryType.PupilID,
                PupilID = "abc"
            };

            var validator = new SearchPupilsViewModelValidator();

            // Act
            var result = validator.Validate(viewModel);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void SearchPupilsViewModel_IsInvalid_GivenSearchTypeIsPupilIDAndTooLongULN()
        {
            // Arrange
            var viewModel = new SearchPupilsViewModel("ks5")
            {
                SearchType = QueryType.PupilID,
                PupilID = "12345678901"
            };

            var validator = new SearchPupilsViewModelValidator();

            // Act
            var result = validator.Validate(viewModel);

            // Assert
            Assert.False(result.IsValid);
        }


        [Fact]
        public void SearchPupilsViewModel_IsValid_GivenSearcgTypeIsNameAndValidName()
        {
            // Arrange
            var viewModel = new SearchPupilsViewModel("ks4")
            {
                SearchType = QueryType.Name,
                Name = "Hector d'Arras Sausage-Hausen, Jr."
            };

            var validator = new SearchPupilsViewModelValidator();

            // Act
            var result = validator.Validate(viewModel);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void SearchPupilsViewModel_IsInvalid_GivenSearcgTypeIsNameAndNoName()
        {
            // Arrange
            var viewModel = new SearchPupilsViewModel("ks4")
            {
                SearchType = QueryType.Name,
                Name = " "
            };

            var validator = new SearchPupilsViewModelValidator();

            // Act
            var result = validator.Validate(viewModel);

            // Assert
            Assert.False(result.IsValid);
        }
        [Fact]
        public void SearchPupilsViewModel_IsInvalid_GivenSearchTypeIsNameAndInvalidNameFormat()
        {
            // Arrange
            var viewModel = new SearchPupilsViewModel("ks4")
            {
                SearchType = QueryType.Name,
                Name = "R2-D2"
            };

            var validator = new SearchPupilsViewModelValidator();

            // Act
            var result = validator.Validate(viewModel);

            // Assert
            Assert.False(result.IsValid);
        }
    }
}
