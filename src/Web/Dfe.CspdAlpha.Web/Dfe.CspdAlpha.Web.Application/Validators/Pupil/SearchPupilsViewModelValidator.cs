using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using FluentValidation;

namespace Dfe.CspdAlpha.Web.Application.Validators.Pupil
{
    public class SearchPupilsViewModelValidator : AbstractValidator<SearchPupilsViewModel>
    {
        public SearchPupilsViewModelValidator()
        {
            RuleFor(x => x.PupilID)
                .NotEmpty()
                .When(x => x.SearchType == QueryType.PupilID)
                .WithMessage("Enter a ULN");

            RuleFor(x => x.Name)
                .NotEmpty()
                .When(x => x.SearchType == QueryType.Name)
                .WithMessage("Enter a student full name");

            RuleFor(x => x.Name)
                .Matches(@"^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$")
                .When(x => x.SearchType == QueryType.Name)
                .WithMessage("Enter a valid student name");
        }
    }
}
