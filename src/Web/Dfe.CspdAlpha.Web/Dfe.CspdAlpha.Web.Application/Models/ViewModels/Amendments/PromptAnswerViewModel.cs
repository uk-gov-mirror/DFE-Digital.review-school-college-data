using System;
using System.Linq;
using Dfe.Rscd.Web.ApiClient;
using Microsoft.AspNetCore.Http;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments
{
    public class PromptAnswerViewModel : ContextAwareViewModel
    {
        public string FieldType { get; set; }
        public Guid QuestionId { get; set; }
        public int CurrentIndex { get; set; }

        public string GetAnswerAsString(IFormCollection fields)
        {
            string answerAsString = string.Empty;

            switch (GetPromptAnswerType())
            {
                case (QuestionType.DateTime):
                    answerAsString = $"{fields["date-day"]}/{fields["date-month"]}/{fields["date-year"]}";
                    break;
                case (QuestionType.Number):
                    answerAsString = $"{fields["integer"]}";
                    break;
                case (QuestionType.Select):
                    answerAsString = fields["listbox"];
                    break;
                case (QuestionType.String):
                    answerAsString = fields["text"];
                    break;
                case (QuestionType.Boolean):
                    answerAsString = fields["yesno"];
                    break;
                case (QuestionType.ConditionalFurther):
                    answerAsString = fields["conditionalfurther"];
                    break;
                case (QuestionType.NestedConditional):
                    answerAsString = fields["nestedconditional"];
                    break;
            }

            return answerAsString;
        }

        private QuestionType GetPromptAnswerType()
        {
            if (!string.IsNullOrEmpty(FieldType))
            {
                if (Enum.GetNames(typeof(QuestionType)).Any(
                    e => e.Trim().ToUpperInvariant() == FieldType.Trim().ToUpperInvariant()))
                {
                    return (QuestionType) Enum.Parse(typeof(QuestionType), FieldType, true);
                }
            }

            throw new Exception("Could not find Answer Type");
        }
    }
}
