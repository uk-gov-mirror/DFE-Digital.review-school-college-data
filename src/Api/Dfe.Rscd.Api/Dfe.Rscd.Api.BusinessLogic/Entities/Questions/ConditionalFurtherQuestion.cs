using System;
using System.Collections.Generic;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
    public class ConditionalFurtherQuestion : Question
    {
        public ConditionalFurtherQuestion(string title, string description, string label,
            List<AnswerPotential> potentials, bool allowNull, string notNullErrorMessage, string notValidErrorMessage, string conditionalValue, Question conditionalQuestion)
        {
            Description = description;
            Title = title;
            Id = Guid.NewGuid();
            QuestionType = QuestionType.ConditionalFurther;
            Answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = Id,
                    AllowNull = allowNull,
                    Label = label,
                    AnswerPotentials = potentials,
                    IsConditional = false,
                    HasConditional = true,
                    ConditionalQuestion = conditionalQuestion,
                    ConditionalValue = conditionalValue,
                    NullErrorMessage = notNullErrorMessage,
                    InValidErrorMessage = notValidErrorMessage,
                    Order = 0,
                },
            };
        }
    }
}
