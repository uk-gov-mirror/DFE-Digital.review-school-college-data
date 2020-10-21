using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using FluentValidation;

namespace Dfe.CspdAlpha.Web.Application.Validators.RemovePupil
{
    public class SearchPupilsViewModelValidator : AbstractValidator<SearchPupilsViewModel>
    {
        public SearchPupilsViewModelValidator()
        {
            RuleFor(x => x.PupilID)
                .NotEmpty()
                .When(x => x.SearchType == QueryType.PupilID)
                .WithMessage("Enter a ULN");

            RuleFor(x => x.PupilID)
                .Matches(@"^[0-9]+$")
                .When(x => x.SearchType == QueryType.PupilID)
                .WithMessage("Enter a valid ULN");

            RuleFor(x => x.PupilID)
                .MaximumLength(10)
                .When(x => x.SearchType == QueryType.PupilID)
                .WithMessage("Enter a valid ULN");

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
