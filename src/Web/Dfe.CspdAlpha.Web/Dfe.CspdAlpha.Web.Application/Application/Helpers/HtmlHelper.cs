using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dfe.Rscd.Web.Application.Application.Helpers
{
    public static class HtmlHelper
    {
        public static IHtmlContent NotRecordedIfNullOrEmpty(this IHtmlHelper htmlHelper, string value)
        {
            return new HtmlString(string.IsNullOrWhiteSpace(value) ? "Not recorded" : value);
        }
    }
}
