using System;
using System.Collections.Generic;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
    public class FutureDateTimeQuestion : Question
    {
        public FutureDateTimeQuestion(string title, string description, string label, bool allowNull, string notNullErrorMessage, string notValidErrorMessage)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            QuestionType = QuestionType.DateTime;
            Answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = Id,
                    AllowNull = allowNull,
                    InValidErrorMessage = notValidErrorMessage,
                    NullErrorMessage = notNullErrorMessage,
                    Label = label,
                    Order = 1,
                    HistoricalDate = false,
                    FutureDate = true
                }
            };

        }
    }
}