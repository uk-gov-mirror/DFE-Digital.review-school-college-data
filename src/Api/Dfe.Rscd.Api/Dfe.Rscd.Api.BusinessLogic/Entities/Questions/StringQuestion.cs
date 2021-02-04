using System;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
    public class StringQuestion : Question
    {
        public StringQuestion(string title, string label, Validator validator)
        {
            Id = Guid.NewGuid();
            Title = title;
            QuestionType = QuestionType.String;
            Validator = validator;
            Answer = new Answer
            {
                QuestionId = Id,
                Label = label
            };
        }
    }
}
