using Dfe.Rscd.Web.Application.Models.ViewModels.RemovePupil;
using FluentValidation;

namespace Dfe.Rscd.Web.Application.Validators.RemovePupil
{
    public class ReasonViewModelValidator : AbstractValidator<ReasonViewModel>
    {
        public ReasonViewModelValidator()
        {
            When(x => x.IsSubReason, () => {
                RuleFor(x => x.SelectedReasonCode)
                    .NotNull()
                    .WithMessage("Select a sub reason");
            }).Otherwise(() => {
                RuleFor(x => x.SelectedReasonCode)
                    .NotNull()
                    .WithMessage("Select a reason");
            });
        }
    }
}
