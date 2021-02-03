using System;
using System.Collections.Generic;
using System.Text;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
    public class HistoricalDateTimeQuestion : Question
    {
        public HistoricalDateTimeQuestion(string title, string description, string label, bool allowNull, string notNullErrorMessage, string notValidErrorMessage)
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
                    HistoricalDate = true,
                    FutureDate = false
                }
            };

        }
    }
}
