using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using Dfe.CspdAlpha.Web.Application.Validators.Pupil;
using Xunit;

namespace Dfe.Rscd.Web.UnitTests.Application.Validators
{
    public class SearchPupilsViewModelValidatorTests
    {
        [Fact]
        public void SearchPupilsViewModel_IsValid_GivenSearchTypeIsPupilIDAndValidPupilID()
        {
            // Arrange
            var viewModel = new SearchPupilsViewModel()
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
            var viewModel = new SearchPupilsViewModel()
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
        public void SearchPupilsViewModel_IsValid_GivenSearcgTypeIsNameAndValidName()
        {
            // Arrange
            var viewModel = new SearchPupilsViewModel()
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
            var viewModel = new SearchPupilsViewModel()
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
        public void SearchPupilsViewModel_IsInvalid_GivenSearcgTypeIsNameAndInvalidNameFormat()
        {
            // Arrange
            var viewModel = new SearchPupilsViewModel()
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
