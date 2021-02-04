using System;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
    public class DateTimeQuestion : Question
    {
        public DateTimeQuestion(string title, string label, Validator validator)
        {
            Id = Guid.NewGuid();
            Title = title;
            QuestionType = QuestionType.DateTime;
            Validator = validator;
            Answer = new Answer
            {
                QuestionId = Id,
                Label = label
            };
        }
    }
}
