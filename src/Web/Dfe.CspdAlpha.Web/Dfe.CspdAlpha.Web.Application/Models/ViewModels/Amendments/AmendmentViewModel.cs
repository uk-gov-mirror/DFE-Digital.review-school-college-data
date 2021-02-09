using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Application.Helpers;
using Dfe.Rscd.Web.Application.Models.Common;

namespace Dfe.Rscd.Web.Application.Models.ViewModels.Amendments
{
    public class AmendmentViewModel : ContextAwareViewModel
    {
        public Amendment Amendment { get; set; }
        public Keystage Keystage => CheckingWindowHelper.ToKeyStage(CheckingWindow);
    }
}
