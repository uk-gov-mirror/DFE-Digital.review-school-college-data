using Dfe.Rscd.Web.Application.Models.ViewModels.AddPupil;
using FluentValidation;

namespace Dfe.Rscd.Web.Application.Validators.AddPupil
{
    public class AddPupilViewModelValidator : AbstractValidator<AddPupilViewModel>
    {
        public AddPupilViewModelValidator()
        {
            When(model => string.IsNullOrWhiteSpace(model.UPN), () =>
            {
                RuleFor(x => x.FirstName)
                    .NotNull()
                    .WithMessage("Enter first name");
                RuleFor(x => x.LastName)
                    .NotNull()
                    .WithMessage("Enter last name");
                RuleFor(x => x.DateOfBirth)
                    .IsValidDate()
                    .WithMessage("Enter date of birth");
                RuleFor(x => x.Gender)
                    .NotNull()
                    .WithMessage("Select gender");
                RuleFor(x => x.DateOfAdmission)
                    .IsValidDate()
                    .WithMessage("Enter date of admission");
                RuleFor(x => x.YearGroup)
                    .NotNull()
                    .WithMessage("Select year group");
            });
        }
    }
}
