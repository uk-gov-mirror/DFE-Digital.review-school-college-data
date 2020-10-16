using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.AddPupil;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Common;
using Dfe.CspdAlpha.Web.Application.Validators.AddPupil;
using System;
using Xunit;

namespace Dfe.Rscd.Web.UnitTests.Application
{
    public class AddPupilViewModelValidatorTests
    {
        [Fact]
        public void AddPupilDetails_IsValid_GivenUPNProvided()
        {
            // Arrange
            var viewModel = new AddPupilViewModel
            {
                UPN = "12345"
            };

            var validator = new AddPupilViewModelValidator();

            // Act
            var result = validator.Validate(viewModel);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void AddPupilDetails_IsValid_GivenValidDetails()
        {
            // Arrange
            var viewModel = new AddPupilViewModel
            {
                FirstName = "Joe",
                LastName = "Bloggs",
                DateOfBirth = new DateViewModel(DateTime.Parse("01/01/2007")),
                Gender = Gender.Male,
                DateOfAdmission = new DateViewModel(DateTime.Parse("01/12/2018")),
                YearGroup = "8"
            };

            var validator = new AddPupilViewModelValidator();

            // Act
            var result = validator.Validate(viewModel);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void AddPupilDetails_IsInvalid_GivenNullFirstName()
        {
            // Arrange
            var viewModel = new AddPupilViewModel
            {
                LastName = "Bloggs",
                DateOfBirth = new DateViewModel(DateTime.Parse("01/01/2007")),
                Gender = Gender.Male,
                DateOfAdmission = new DateViewModel(DateTime.Parse("01/12/2018")),
                YearGroup = "8"
            };

            var validator = new AddPupilViewModelValidator();

            // Act
            var result = validator.Validate(viewModel);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void AddPupilDetails_IsInvalid_GivenNullDateOfBirth()
        {
            // Arrange
            var viewModel = new AddPupilViewModel
            {
                FirstName = "Joe",
                LastName = "Bloggs",
                DateOfBirth = new DateViewModel(),
                Gender = Gender.Male,
                DateOfAdmission = new DateViewModel(DateTime.Parse("01/12/2018")),
                YearGroup = "8"
            };

            var validator = new AddPupilViewModelValidator();

            // Act
            var result = validator.Validate(viewModel);

            // Assert
            Assert.False(result.IsValid);
        }
    }
}
