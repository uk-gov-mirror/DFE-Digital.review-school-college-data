using Dfe.CspdAlpha.Web.Application.Models.Common;
using Microsoft.AspNetCore.Routing;

namespace Dfe.CspdAlpha.Web.Application.Application.Helpers
{
    public class CheckingWindowHelper
    {
        public static CheckingWindow GetCheckingWindow(RouteData routeData)
        {
            switch (routeData.Values["phase"].ToString())
            {
                case "ks4-june":
                    return CheckingWindow.KS4June;
                case "ks4-late":
                    return CheckingWindow.KS4Late;
                default:
                    return CheckingWindow.Unknown;
            }
        }

        public static string GetCheckingWindowDescription(CheckingWindow checkingWindow)
        {
            switch (checkingWindow)
            {
                case CheckingWindow.Unknown:
                    break;
                case CheckingWindow.KS2:
                    break;
                case CheckingWindow.KS2Errata:
                    break;
                case CheckingWindow.KS4June:
                    return "Key stage 4 June checking exercise";
                case CheckingWindow.KS4Late:
                    return "Key stage 4 Late results";
                case CheckingWindow.KS4Errata:
                    break;
                case CheckingWindow.KS5:
                    break;
                case CheckingWindow.KS5Errata:
                    break;
                default:
                    break;
            }
            return string.Empty;
        }
    }
}
