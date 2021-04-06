using System;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
    public class StringQuestion : Question
    {
        public StringQuestion(string id, string title, string label, Validator validator, string subLabel = "")
        {
            Id = id;
            Title = title;
            QuestionType = QuestionType.String;
            Validator = validator;
            Answer = new Answer
            {
                Label = label,
                SubLabel = subLabel
            };
        }
    }
}
