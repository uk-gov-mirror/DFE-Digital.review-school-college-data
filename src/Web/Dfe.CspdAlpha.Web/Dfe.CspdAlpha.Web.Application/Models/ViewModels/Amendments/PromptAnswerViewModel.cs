using System;
using System.Linq;
using Dfe.Rscd.Web.ApiClient;
using Microsoft.AspNetCore.Http;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments
{
    public class PromptAnswerViewModel : ContextAwareViewModel
    {
        public string FieldType { get; set; }
        public int PromptId { get; set; }
        public int CurrentIndex { get; set; }

        public PromptAnswer GetPromptAnswer(IFormCollection fields)
        {
            return new PromptAnswer
            {
                PromptAnswerType = GetPromptAnswerType(),
                PromptID = PromptId,
                PromptStringAnswer = GetAnswerAsString(fields)
            };
        }

        public string GetAnswerAsString(IFormCollection fields)
        {
            string answerAsString = string.Empty;

            switch (GetPromptAnswerType())
            {
                case (PromptAnswerTypeEnum.Date):
                    answerAsString = $"{fields["date-day"]}/{fields["date-month"]}/{fields["date-year"]}";
                    break;
                case (PromptAnswerTypeEnum.Info):
                    //No answer required.
                    break;
                case (PromptAnswerTypeEnum.Integer):
                    answerAsString = $"{fields["integer"]}";
                    break;
                case (PromptAnswerTypeEnum.ListBox):
                    answerAsString = fields["listbox"];
                    break;
                case (PromptAnswerTypeEnum.Text):
                    answerAsString = fields["text"];
                    break;
                case (PromptAnswerTypeEnum.YesNo):
                    answerAsString = fields["yesno"];
                    break;
                default:
                    //Should never occur.
                    break;
            }

            return answerAsString;
        }

        private PromptAnswerTypeEnum GetPromptAnswerType()
        {
            if (!string.IsNullOrEmpty(FieldType))
            {
                if (Enum.GetNames(typeof(PromptAnswerTypeEnum)).Any(
                    e => e.Trim().ToUpperInvariant() == FieldType.Trim().ToUpperInvariant()))
                {
                    return (PromptAnswerTypeEnum) Enum.Parse(typeof(PromptAnswerTypeEnum), FieldType, true);
                }
            }

            throw new Exception("Could not find Answer Type");
        }
    }
}
