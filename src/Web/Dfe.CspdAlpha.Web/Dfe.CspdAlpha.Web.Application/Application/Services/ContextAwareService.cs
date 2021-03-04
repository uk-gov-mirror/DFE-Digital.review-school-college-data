using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Application.Helpers;

namespace Dfe.Rscd.Web.Application.Application.Services
{
    public class ContextAwareService
    {
        protected CheckingWindow CheckingWindow =>
            CheckingWindowHelper.GetCheckingWindow(Context.Current.Request.RouteValues);
    }
}
