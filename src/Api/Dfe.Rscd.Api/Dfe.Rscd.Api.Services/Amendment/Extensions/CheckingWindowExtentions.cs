using System;
using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services
{
    public static class CheckingWindowExtentions
    {
        public static CheckingWindow ToDomainCheckingWindow(this string checkingWindowName)
        {
            Enum.TryParse(checkingWindowName.Replace("-", string.Empty), true, out CheckingWindow checkingWindow);
            return checkingWindow;
        }

        public static rscd_Checkingwindow ToCRMCheckingWindow(this CheckingWindow checkingwindow)
        {
            switch (checkingwindow)
            {
                case CheckingWindow.KS2:
                    return rscd_Checkingwindow.KS2;
                case CheckingWindow.KS2Errata:
                    return rscd_Checkingwindow.KS2Errata;
                case CheckingWindow.KS4June:
                    return rscd_Checkingwindow.KS4June;
                case CheckingWindow.KS4Late:
                    return rscd_Checkingwindow.KS4Late;
                case CheckingWindow.KS4Errata:
                    return rscd_Checkingwindow.KS4Errata;
                case CheckingWindow.KS5:
                    return rscd_Checkingwindow.KS5;
                case CheckingWindow.KS5Errata:
                    return rscd_Checkingwindow.KS5Errata;
                default:
                    throw new ApplicationException();
            }
        }

        public static CheckingWindow ToDomainCheckingWindow(this rscd_Checkingwindow checkingwindow)
        {
            switch (checkingwindow)
            {
                case rscd_Checkingwindow.KS2:
                    return CheckingWindow.KS2;
                case rscd_Checkingwindow.KS2Errata:
                    return CheckingWindow.KS2Errata;
                case rscd_Checkingwindow.KS4June:
                    return CheckingWindow.KS4June;
                case rscd_Checkingwindow.KS4Late:
                    return CheckingWindow.KS4Late;
                case rscd_Checkingwindow.KS4Errata:
                    return CheckingWindow.KS4Errata;
                case rscd_Checkingwindow.KS5:
                    return CheckingWindow.KS5;
                case rscd_Checkingwindow.KS5Errata:
                    return CheckingWindow.KS5Errata;
                default:
                    throw new ApplicationException();
            }
        }
    }
}