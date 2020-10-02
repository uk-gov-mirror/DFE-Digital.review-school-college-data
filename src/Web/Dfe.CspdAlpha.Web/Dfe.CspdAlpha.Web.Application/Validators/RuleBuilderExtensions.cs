using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Common;
using FluentValidation;

namespace Dfe.CspdAlpha.Web.Application.Validators
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, DateViewModel> IsValidDate<T>(this IRuleBuilder<T, DateViewModel> rule)
        {
            return rule.Must(dateViewModel => dateViewModel.Date != null);
        }
    }
}
