using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using FluentValidation;

namespace Dfe.CspdAlpha.Web.Application.Validators.RemovePupil
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
