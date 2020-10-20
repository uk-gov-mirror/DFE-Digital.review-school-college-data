using Dfe.CspdAlpha.Web.Application.Models.Common;
using Microsoft.Crm.Sdk.Messages;
using ApiCheckingWindow = Dfe.Rscd.Web.ApiClient.CheckingWindow;
namespace Dfe.CspdAlpha.Web.Application.Application.Extensions
{
    public static class CheckingWindowExtensions
    {
        public static CheckingWindow ToApplicationCheckingWindow(
            this ApiCheckingWindow checkingWindow)
        {
            switch (checkingWindow)
            {
                case ApiCheckingWindow.KS2:
                    return CheckingWindow.KS2;
                case ApiCheckingWindow.KS2Errata:
                    return CheckingWindow.KS2Errata;
                case ApiCheckingWindow.KS4June:
                    return CheckingWindow.KS4June;
                case ApiCheckingWindow.KS4Late:
                    return CheckingWindow.KS4Late;
                case ApiCheckingWindow.KS4Errata:
                    return CheckingWindow.KS4Errata;
                case ApiCheckingWindow.KS5:
                    return CheckingWindow.KS5;
                case ApiCheckingWindow.KS5Errata:
                    return CheckingWindow.KS5Errata;
            }

            return CheckingWindow.Unknown;
        }
        public static ApiCheckingWindow ToApiCheckingWindow(
            this CheckingWindow checkingWindow)
        {
            switch (checkingWindow)
            {
                case CheckingWindow.KS2:
                    return ApiCheckingWindow.KS2;
                case CheckingWindow.KS2Errata:
                    return ApiCheckingWindow.KS2Errata;
                case CheckingWindow.KS4June:
                    return ApiCheckingWindow.KS4June;
                case CheckingWindow.KS4Late:
                    return ApiCheckingWindow.KS4Late;
                case CheckingWindow.KS4Errata:
                    return ApiCheckingWindow.KS4Errata;
                case CheckingWindow.KS5:
                    return ApiCheckingWindow.KS5;
                case CheckingWindow.KS5Errata:
                    return ApiCheckingWindow.KS5Errata;
            }

            return ApiCheckingWindow.Unknown;
        }

        public static string ToCheckingWindowLabel(this CheckingWindow checkingWindow)
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

        public static Keystage ToKeyStage(this CheckingWindow checkingWindow)
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

        public static Keystage ToKeyStage(this ApiCheckingWindow checkingWindow)
        {
            switch (checkingWindow)
            {
                case ApiCheckingWindow.KS2:
                case ApiCheckingWindow.KS2Errata:
                    return Keystage.KS2;
                case ApiCheckingWindow.KS4June:
                case ApiCheckingWindow.KS4Late:
                case ApiCheckingWindow.KS4Errata:
                    return Keystage.KS4;
                case ApiCheckingWindow.KS5:
                case ApiCheckingWindow.KS5Errata:
                    return Keystage.KS5;
                default:
                    return Keystage.Unknown;
            }
        }


    }
}
