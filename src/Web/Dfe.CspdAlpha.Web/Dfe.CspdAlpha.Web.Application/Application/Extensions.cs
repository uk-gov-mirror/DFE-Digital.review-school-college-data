using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Application
{
    public static class Extensions
    {
        public static Keystage ToKeyStage(this CheckingWindow checkingWindow)
        {
            var checkingWindowString = checkingWindow.ToString();
            if (checkingWindowString.ToLower().StartsWith("ks2")) return Keystage.KS2;
            if (checkingWindowString.ToLower().StartsWith("ks4")) return Keystage.KS4;
            if (checkingWindowString.ToLower().StartsWith("ks5")) return Keystage.KS5;

            return Keystage.Unknown;
        }
    }
}
