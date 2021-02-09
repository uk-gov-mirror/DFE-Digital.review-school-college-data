using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Models.ViewModels.RemovePupil;
using FluentValidation;

namespace Dfe.Rscd.Web.Application.Validators.RemovePupil
{
    public class SearchPupilsViewModelValidator : AbstractValidator<SearchPupilsViewModel>
    {
        public SearchPupilsViewModelValidator()
        {
            RuleFor(x => x.PupilID)
                .NotEmpty()
                .When(x => x.SearchType == QueryType.PupilID && x.CheckingWindow != CheckingWindow.KS5)
                .WithMessage("Enter a UPN");

            RuleFor(x => x.PupilID)
                .Matches(@"^[a-zA-Z]+[a-zA-Z0-9]*$")
                .When(x => x.SearchType == QueryType.PupilID && x.CheckingWindow != CheckingWindow.KS5)
                .WithMessage("Enter a valid UPN");

            RuleFor(x => x.PupilID)
                .MaximumLength(14)
                .When(x => x.SearchType == QueryType.PupilID && x.CheckingWindow != CheckingWindow.KS5)
                .WithMessage("Enter a valid UPN");

            RuleFor(x => x.Name)
                .NotEmpty()
                .When(x => x.SearchType == QueryType.Name && x.CheckingWindow != CheckingWindow.KS5)
                .WithMessage("Enter a pupil full name");

            RuleFor(x => x.Name)
                .MinimumLength(2)
                .When(x => x.SearchType == QueryType.Name && x.CheckingWindow != CheckingWindow.KS5)
                .WithMessage("Enter a valid pupil name");

            RuleFor(x => x.Name)
                .Matches(@"^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$")
                .When(x => x.SearchType == QueryType.Name && x.CheckingWindow != CheckingWindow.KS5)
                .WithMessage("Enter a valid pupil name");


            RuleFor(x => x.PupilID)
                .NotEmpty()
                .When(x => x.SearchType == QueryType.PupilID && x.CheckingWindow == CheckingWindow.KS5)
                .WithMessage("Enter a ULN");

            RuleFor(x => x.PupilID)
                .Matches(@"^[0-9]+$")
                .When(x => x.SearchType == QueryType.PupilID && x.CheckingWindow == CheckingWindow.KS5)
                .WithMessage("Enter a valid ULN");

            RuleFor(x => x.PupilID)
                .MaximumLength(10)
                .When(x => x.SearchType == QueryType.PupilID && x.CheckingWindow == CheckingWindow.KS5)
                .WithMessage("Enter a valid ULN");

            RuleFor(x => x.Name)
                .NotEmpty()
                .When(x => x.SearchType == QueryType.Name && x.CheckingWindow == CheckingWindow.KS5)
                .WithMessage("Enter a student full name");

            RuleFor(x => x.Name)
                .MinimumLength(2)
                .When(x => x.SearchType == QueryType.Name && x.CheckingWindow == CheckingWindow.KS5)
                .WithMessage("Enter a valid student name");

            RuleFor(x => x.Name)
                .Matches(@"^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$")
                .When(x => x.SearchType == QueryType.Name && x.CheckingWindow == CheckingWindow.KS5)
                .WithMessage("Enter a valid student name");
        }
    }
}
