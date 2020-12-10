using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using FluentValidation;

namespace Dfe.CspdAlpha.Web.Application.Validators.RemovePupil
{
    public class AllocationYearViewModelValidation : AbstractValidator<AllocationYearViewModel>
    {
        public AllocationYearViewModelValidation()
        {
            RuleFor(x => x.SelectedAllocationYears)
                .NotEmpty()
                .WithMessage("Select an allocation year");
        }
    }
}
