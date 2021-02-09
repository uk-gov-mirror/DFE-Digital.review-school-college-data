using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Models.Common;
using Microsoft.AspNetCore.Routing;

namespace Dfe.Rscd.Web.Application.Application.Helpers
{
    public class CheckingWindowHelper
    {
        public static CheckingWindow GetCheckingWindow(RouteData routeData)
        {
            return GetCheckingWindow(routeData.Values["phase"].ToString());
        }

        public static CheckingWindow GetCheckingWindow(RouteValueDictionary routeData)
        {
            return GetCheckingWindow(routeData["phase"].ToString());
        }

        public static CheckingWindow GetCheckingWindow(string phase)
        {
            switch (phase)
            {
                case "ks4-june":
                    return CheckingWindow.KS4June;
                case "ks4-late":
                    return CheckingWindow.KS4Late;
                case "ks5":
                    return CheckingWindow.KS5;
                default:
                    return CheckingWindow.Unknown;
            }
        }

        public static string GetCheckingWindowURL(CheckingWindow checkingWindow)
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
                    return "ks4-june";
                case CheckingWindow.KS4Late:
                    return "ks4-late";
                case CheckingWindow.KS4Errata:
                    break;
                case CheckingWindow.KS5:
                    return "ks5";
                case CheckingWindow.KS5Errata:
                    break;
            }

            return string.Empty;
        }

        public static Keystage ToKeyStage(CheckingWindow checkingWindow)
        {
            switch (checkingWindow)
            {
                case CheckingWindow.KS2:
                case CheckingWindow.KS2Errata:
                    return Keystage.KS2;
                case CheckingWindow.KS4June:
                case CheckingWindow.KS4Late:
                case CheckingWindow.KS4Errata:
                    return Keystage.KS4;
                case CheckingWindow.KS5:
                case CheckingWindow.KS5Errata:
                    return Keystage.KS5;
                default:
                    return Keystage.Unknown;
            }
        }

        public static string ToCheckingWindowLabel(CheckingWindow checkingWindow)
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
                    return "June";
                case CheckingWindow.KS4Late:
                    return "Sep";
                case CheckingWindow.KS4Errata:
                    break;
                case CheckingWindow.KS5:
                    return "Sep";
                case CheckingWindow.KS5Errata:
                    break;
                default:
                    break;
            }
            return string.Empty;
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
                    return "16 to 18 September checking exercise";
                case CheckingWindow.KS5Errata:
                    break;
            }

            return string.Empty;
        }
    }
}
