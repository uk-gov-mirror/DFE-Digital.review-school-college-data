using System;

namespace Dfe.Rscd.Api.BusinessLogic.Entities
{
    public class PromptAnswer
    {

        public string ErrorMessage { get; set; }
        public string InformationMessage { get; set; }
        public string WarningMessage { get; set; }

        public PromptAnswer(int promptId)
        {
            PromptID = promptId;
            PromptText = "";
            ColumnType = "";
            PromptShortText = "";
        }

        public PromptAnswer(int promptId, string promptText, string promptShortText, Int16? updatedByDA)
        {
            PromptID = promptId;
            PromptText = promptText;
            ColumnType = "";
            PromptShortText = promptShortText;
            UpdatedByDA = updatedByDA;
        }

        public int PromptID { get; set; }

        public string PromptText { get; set; }

        public string ColumnType { get; set; }

        public PromptAnswerTypeEnum PromptAnswerType { get; set; }

        public string PromptSelectedValueAnswer { get; set; }

        public string PromptStringAnswer { get; set; }

        public int? PromptIntegerAnswer { get; set; }

        public DateTime? PromptDateTimeAnswer { get; set; }

        public bool? PromptYesNoAnswer { get; set; }

        public bool? PromptAcknowledgeInfoSightAnswer { get; set; }

        public bool AllowNulls { get; set; }

        public string PromptShortText { get; set; }

        public Int16? UpdatedByDA { get; set; }

        public enum PromptAnswerTypeEnum
        {
            ListBox = 1,
            Date = 2,
            Integer = 3,
            Text = 4,
            YesNo = 5,
            Info = 6
        }

        public string GetAnswerAsString()
        {
            string answerAsString = string.Empty;

            switch (this.PromptAnswerType)
            {
                case (PromptAnswer.PromptAnswerTypeEnum.Date):
                    answerAsString = this.PromptDateTimeAnswer.ToString();
                    break;
                case (PromptAnswer.PromptAnswerTypeEnum.Info):
                    //No answer required.
                    break;
                case (PromptAnswer.PromptAnswerTypeEnum.Integer):
                    answerAsString = this.PromptIntegerAnswer.ToString();
                    break;
                case (PromptAnswer.PromptAnswerTypeEnum.ListBox):
                    answerAsString = this.PromptSelectedValueAnswer.ToString();
                    break;
                case (PromptAnswer.PromptAnswerTypeEnum.Text):
                    answerAsString = this.PromptStringAnswer;
                    break;
                case (PromptAnswer.PromptAnswerTypeEnum.YesNo):
                    answerAsString = (this.PromptYesNoAnswer.Value) ? "Yes" : "No";
                    break;
                default:
                    //Should never occur.
                    break;
            }

            return answerAsString;
        }
    }
}
