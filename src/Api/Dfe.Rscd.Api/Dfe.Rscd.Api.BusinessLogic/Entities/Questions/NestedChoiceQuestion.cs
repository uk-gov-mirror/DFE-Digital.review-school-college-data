using System;
using System.Collections.Generic;
using System.Text;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
    public class NestedChoiceQuestion : Question
    {
        public NestedChoiceQuestion(string title, string description, string label, 
            List<AnswerPotential> potentials, bool allowNull, string notNullErrorMessage, string notValidErrorMessage, string conditionalValue, Question nestedQuestion)
        {
            Description = description;
            Title = title;
            Id = Guid.NewGuid();
            QuestionType = QuestionType.NestedConditional;
            Answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = Id,
                    AllowNull = allowNull,
                    Label = label,
                    AnswerPotentials = potentials,
                    IsConditional = true,
                    ConditionalValue = conditionalValue,
                    ConditionalQuestion = nestedQuestion,
                    NullErrorMessage = notNullErrorMessage,
                    InValidErrorMessage = notValidErrorMessage,
                    Order = 0
                },
            };
        }
    }
}
