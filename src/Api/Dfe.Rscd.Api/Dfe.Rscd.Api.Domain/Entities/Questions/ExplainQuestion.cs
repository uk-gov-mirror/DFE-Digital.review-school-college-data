namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
    public class ExplainQuestion : StringQuestion
    {
        public ExplainQuestion(string id, string title, string label,string subLabel, Validator validator) 
            : base(id, title, label, validator, subLabel)
        {
            QuestionType = QuestionType.Explain;
            Answer = new Answer
            {
                Label = label,
                SubLabel = subLabel
            };
        }
    }
}