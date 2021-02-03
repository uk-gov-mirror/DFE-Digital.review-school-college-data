using System;
using System.Collections.Generic;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
    public class NumberQuestion : Question
    {
        public NumberQuestion(string title, string description, string label, bool allowNull, string notNullErrorMessage, string notValidErrorMessage)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            QuestionType = QuestionType.Number;
            Answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = Id,
                    AllowNull = allowNull,
                    InValidErrorMessage = notValidErrorMessage,
                    NullErrorMessage = notNullErrorMessage,
                    ValidationRegex = @"^[1-9]\d*$",
                    Label = label,
                    Order = 1
                }
            };

        }
    }
}
