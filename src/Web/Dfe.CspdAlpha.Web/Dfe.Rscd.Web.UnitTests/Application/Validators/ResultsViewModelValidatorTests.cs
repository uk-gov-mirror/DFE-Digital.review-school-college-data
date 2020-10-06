﻿using System;
using System.Collections.Generic;
using System.Text;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using Dfe.CspdAlpha.Web.Application.Validators.Pupil;
using Dfe.CspdAlpha.Web.Application.Validators.RemovePupil;
using Xunit;

namespace Dfe.Rscd.Web.UnitTests.Application.Validators
{
    public class ResultsViewModelValidatorTests
    {
        [Fact]
        public void ResultsViewModel_IsValid_GivenSelectedID()
        {
            // Arrange
            var viewModel = new ResultsViewModel()
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
            var viewModel = new ResultsViewModel()
            {
                SelectedID = null
            };

            var validator = new ResultsViewModelValidator();

            // Act
            var result = validator.Validate(viewModel);

            // Assert
            Assert.False(result.IsValid);
        }
    }
}
