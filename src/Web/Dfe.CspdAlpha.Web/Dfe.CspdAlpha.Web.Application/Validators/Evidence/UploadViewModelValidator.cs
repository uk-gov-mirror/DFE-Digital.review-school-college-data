using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Evidence;
using FluentValidation;

namespace Dfe.CspdAlpha.Web.Application.Validators.Evidence
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
