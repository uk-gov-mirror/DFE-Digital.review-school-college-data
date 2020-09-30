using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace Dfe.CspdAlpha.Web.Application.TagHelpers
{
    /// <summary>
    /// Adds GOV.UK Frontend validation markup to the input element.
    /// </summary>
    [HtmlTargetElement("input", Attributes = "asp-for", TagStructure = TagStructure.WithoutEndTag)]
    public class InputTagHelper : TagHelper
    {
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            ModelStateEntry entry;

            ViewContext.ViewData.ModelState.TryGetValue(For.Name, out entry);

            if (entry != null && entry.Errors.Count > 0)
            {
                var builder = new TagBuilder("input");

                builder.AddCssClass("govuk-input--error");
                output.MergeAttributes(builder);
                output.RemoveClass("input-validation-error", HtmlEncoder.Default);

                output.PreElement.AppendFormat("<span id=\"{0}-error\" class=\"govuk-error-message\">" +
                    "<span class=\"govuk-visually-hidden\">Error:</span> {1}</span>",
                    For.Name, entry.Errors[0].ErrorMessage);
            }
        }
    }
}
