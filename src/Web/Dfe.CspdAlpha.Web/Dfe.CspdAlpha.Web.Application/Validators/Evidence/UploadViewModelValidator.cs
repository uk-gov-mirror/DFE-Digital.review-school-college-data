using Dfe.Rscd.Web.Application.Models.ViewModels.Evidence;
using FluentValidation;

namespace Dfe.Rscd.Web.Application.Validators.Evidence
{
    public class UploadViewModelValidator: AbstractValidator<UploadViewModel>
    {
        public UploadViewModelValidator()
        {
            RuleFor(x => x.EvidenceFiles)
                .NotEmpty()
                .WithMessage("Upload file");
        }
    }
}
