using System;
using System.Collections.Generic;
using System.Text;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
    public class BooleanQuestion : Question
    {
        public BooleanQuestion(string title, string description, string label, bool allowNull, string notNullErrorMessage, string notValidErrorMessage)
        {
            Description = description;
            Title = title;
            Id = Guid.NewGuid();
            QuestionType = QuestionType.Boolean;
            Answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = Id,
                    AllowNull = allowNull,
                    Label = label,
                    AnswerPotentials = new List<AnswerPotential>
                    {
                        new AnswerPotential
                        {
                            Description = "Yes",
                            Value = "1"
                        },
                        new AnswerPotential
                        {
                            Description = "No",
                            Value = "0"
                        }
                    },
                    IsConditional = false,
                    NullErrorMessage = notNullErrorMessage,
                    InValidErrorMessage = notValidErrorMessage,
                    Order = 0,
                },
            };
        }
    }
}
