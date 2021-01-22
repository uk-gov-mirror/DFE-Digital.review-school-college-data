namespace Dfe.Rscd.Api.BusinessLogic.Entities
{
    public class PromptAnswer
    {
        public int PromptID { get; set; }

        public PromptAnswerTypeEnum PromptAnswerType { get; set; }

        public string PromptStringAnswer { get; set; }

        public enum PromptAnswerTypeEnum
        {
            ListBox = 1,
            Date = 2,
            Integer = 3,
            Text = 4,
            YesNo = 5,
            Info = 6
        }
    }
}
