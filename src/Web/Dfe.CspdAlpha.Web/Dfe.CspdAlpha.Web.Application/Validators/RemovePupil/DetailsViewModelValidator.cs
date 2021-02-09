using Dfe.Rscd.Web.Application.Models.ViewModels.RemovePupil;
using FluentValidation;

namespace Dfe.Rscd.Web.Application.Validators.RemovePupil
{
    public class DetailsViewModelValidator : AbstractValidator<DetailsViewModel>
    {
        public DetailsViewModelValidator()
        {
            RuleFor(x => x.AmendmentDetails)
                .NotEmpty()
                .WithMessage("Provide details");

            RuleFor(x => x.AmendmentDetails)
                .MaximumLength(500)
                .WithMessage("A maximum of 500 characters is allowed");
        }
    }
}
