using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using FluentValidation;

namespace Dfe.CspdAlpha.Web.Application.Validators.RemovePupil
{
    public class SubReasonViewModelValidator : AbstractValidator<SubReasonViewModel>
    {
        public SubReasonViewModelValidator()
        {
            RuleFor(x => x.SelectedReason)
                .NotNull()
                .WithMessage("Select a sub reason");
        }
    }
}
