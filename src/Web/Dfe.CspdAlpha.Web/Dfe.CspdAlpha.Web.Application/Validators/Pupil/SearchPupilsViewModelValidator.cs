using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using FluentValidation;

namespace Dfe.CspdAlpha.Web.Application.Validators.Pupil
{
    public class SearchPupilsViewModelValidator : AbstractValidator<SearchPupilsViewModel>
    {
        public SearchPupilsViewModelValidator()
        {
            RuleFor(x => x.SearchType)
                .NotEmpty()
                .WithMessage("Select a search option");

            RuleFor(x => x.PupilID)
                .NotEmpty()
                .When(x => x.SearchType == "PupilID")
                .WithMessage("Enter a ULN");

            RuleFor(x => x.Name)
                .NotEmpty()
                .When(x => x.SearchType == "Name")
                .WithMessage("Enter a student full name");

            RuleFor(x => x.Name)
                .Matches(@"^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$")
                .When(x => x.SearchType == "Name")
                .WithMessage("Enter a valid student name");
        }
    }
}
