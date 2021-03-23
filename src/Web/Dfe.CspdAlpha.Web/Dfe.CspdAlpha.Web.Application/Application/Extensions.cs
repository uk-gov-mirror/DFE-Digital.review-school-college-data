using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Models.Common;

namespace Dfe.Rscd.Web.Application.Application
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

        public static string Clean(this string text) => string.IsNullOrWhiteSpace(text) ? null : text.Trim();
    }
}
