using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using Dfe.Rscd.Web.ApiClient;
using FluentValidation;

namespace Dfe.CspdAlpha.Web.Application.Validators.RemovePupil
{
    public class ResultsViewModelValidator : AbstractValidator<ResultsViewModel>
    {
        public ResultsViewModelValidator()
        {
            RuleFor(x => x.SelectedID)
                .NotNull()
                .When(x=>x.CheckingWindow == CheckingWindow.KS5)
                .WithMessage("Select a student");


            RuleFor(x => x.SelectedID)
                .NotNull()
                .When(x=> x.CheckingWindow != CheckingWindow.KS5)
                .WithMessage("Select a pupil");
        }
    }
}
