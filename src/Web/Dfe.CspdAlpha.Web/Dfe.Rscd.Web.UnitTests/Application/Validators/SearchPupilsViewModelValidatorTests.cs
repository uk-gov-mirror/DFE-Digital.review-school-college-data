using Dfe.Rscd.Web.Application.Models.ViewModels.RemovePupil;
using Dfe.Rscd.Web.Application.Validators.RemovePupil;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Dfe.Rscd.Web.UnitTests.Application.Validators
{
    public class SearchPupilsViewModelValidatorTests
    {
        [Theory]
        [InlineData("ks5")]
        public void SearchPupilsViewModel_IsValid_GivenSearchTypeIsPupilIDAndValidPupilIDKS5(string check)
        {
            // Arrange
            var viewModel = new SearchPupilsViewModel(check)
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

        [Theory]
        [InlineData("ks4-june")]
        [InlineData("ks4-late")]
        public void SearchPupilsViewModel_IsValid_GivenSearchTypeIsPupilIDAndValidPupilID(string check)
        {
            // Arrange
            var viewModel = new SearchPupilsViewModel(check)
            {
                SearchType = QueryType.PupilID,
                PupilID = "D79"
            };

            var validator = new SearchPupilsViewModelValidator();

            // Act
            var result = validator.Validate(viewModel);

            // Assert
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("ks5")]
        [InlineData("ks4-june")]
        [InlineData("ks4-late")]
        public void SearchPupilsViewModel_IsInvalid_GivenSearchTypeIsPupilIDAndNoPupilID(string check)
        {
            // Arrange
            var viewModel = new SearchPupilsViewModel(check)
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

        [Theory]
        [InlineData("ks5")]
        public void SearchPupilsViewModel_IsInvalid_GivenSearchTypeIsPupilIDAndInvalidULN(string check)
        {
            // Arrange
            var viewModel = new SearchPupilsViewModel(check)
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

        [Theory]
        [InlineData("ks5")]
        [InlineData("ks4-june")]
        [InlineData("ks4-late")]
        public void SearchPupilsViewModel_IsInvalid_GivenSearchTypeIsPupilIDAndTooLongULN(string check)
        {
            // Arrange
            var viewModel = new SearchPupilsViewModel(check)
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


        [Theory]
        [InlineData("ks5")]
        [InlineData("ks4-june")]
        [InlineData("ks4-late")]
        public void SearchPupilsViewModel_IsValid_GivenSearcgTypeIsNameAndValidName(string check)
        {
            // Arrange
            var viewModel = new SearchPupilsViewModel(check)
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

        [Theory]
        [InlineData("ks5")]
        [InlineData("ks4-june")]
        [InlineData("ks4-late")]
        public void SearchPupilsViewModel_IsInvalid_GivenSearcgTypeIsNameAndNoName(string check)
        {
            // Arrange
            var viewModel = new SearchPupilsViewModel("ks5")
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

        [Theory]
        [InlineData("ks5")]
        [InlineData("ks4-june")]
        [InlineData("ks4-late")]
        public void SearchPupilsViewModel_IsInvalid_GivenSearcgTypeIsNameAndNotAtLeast2Chars(string check)
        {
            // Arrange
            var viewModel = new SearchPupilsViewModel(check)
            {
                SearchType = QueryType.Name,
                Name = "h"
            };

            var validator = new SearchPupilsViewModelValidator();

            // Act
            var result = validator.Validate(viewModel);

            // Assert
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("ks5")]
        [InlineData("ks4-june")]
        [InlineData("ks4-late")]
        public void SearchPupilsViewModel_IsInvalid_GivenSearcgTypeIsNameAndAtLeast2Chars(string check)
        {
            // Arrange
            var viewModel = new SearchPupilsViewModel(check)
            {
                SearchType = QueryType.Name,
                Name = "hh"
            };

            var validator = new SearchPupilsViewModelValidator();

            // Act
            var result = validator.Validate(viewModel);

            // Assert
            Assert.True(result.IsValid);
        }
        
        [Theory]
        [InlineData("ks5")]
        [InlineData("ks4-june")]
        [InlineData("ks4-late")]
        public void SearchPupilsViewModel_IsInvalid_GivenSearchTypeIsNameAndInvalidNameFormat(string check)
        {
            // Arrange
            var viewModel = new SearchPupilsViewModel(check)
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
