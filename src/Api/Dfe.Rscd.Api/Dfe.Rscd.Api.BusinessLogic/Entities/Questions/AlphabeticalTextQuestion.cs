using System;
using System.Collections.Generic;
using System.Text;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
    public class AlphabeticalTextQuestion : Question
    {
        public AlphabeticalTextQuestion(string title, string description, string label, bool allowNull, string notNullErrorMessage, string notValidErrorMessage)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            QuestionType = QuestionType.String;
            Answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = Id,
                    AllowNull = allowNull,
                    InValidErrorMessage = notValidErrorMessage,
                    NullErrorMessage = notNullErrorMessage,
                    ValidationRegex = @"/^[A-Za-z]+$/",
                    Label = label,
                    Order = 1
                }
            };

        }
    }
}
