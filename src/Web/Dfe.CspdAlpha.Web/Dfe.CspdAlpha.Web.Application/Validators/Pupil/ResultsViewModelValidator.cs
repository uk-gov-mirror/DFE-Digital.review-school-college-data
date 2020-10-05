using System;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using FluentValidation;

namespace Dfe.CspdAlpha.Web.Application.Validators.Pupil
{
    public class ResultsViewModelValidator : AbstractValidator<ResultsViewModel>
    {
        public ResultsViewModelValidator()
        {
            RuleFor(x => x.SelectedID)
                .NotNull()
                .WithMessage("Select a student");
        }
    }
}
