using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using FluentValidation;

namespace Dfe.CspdAlpha.Web.Application.Validators.RemovePupil
{
    public class AllocationYearViewModelValidation : AbstractValidator<AllocationYearViewModel>
    {
        public AllocationYearViewModelValidation()
        {
            RuleFor(x => x.AllocationYear)
                .NotNull()
                .WithMessage("Select an allocation year");
        }
    }
}
