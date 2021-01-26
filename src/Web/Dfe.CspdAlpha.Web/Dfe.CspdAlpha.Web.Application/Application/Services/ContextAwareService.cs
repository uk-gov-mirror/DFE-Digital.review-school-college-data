using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Application.Services
{
    public class ContextAwareService
    {
        protected CheckingWindow CheckingWindow =>
            CheckingWindowHelper.GetCheckingWindow(Context.Current.Request.RouteValues);

        protected string CheckingWindowUrl => CheckingWindowHelper.GetCheckingWindowURL(CheckingWindow);
    }
}
