using Dfe.Rscd.Web.Application.Models.ViewModels.RemovePupil;
using FluentValidation;

namespace Dfe.Rscd.Web.Application.Validators.RemovePupil
{
    public class ReasonViewModelValidator : AbstractValidator<ReasonViewModel>
    {
        public ReasonViewModelValidator()
        {
            RuleFor(x => x.SelectedReasonCode)
                .NotNull()
                .WithMessage("Select a reason");
        }
    }
}
