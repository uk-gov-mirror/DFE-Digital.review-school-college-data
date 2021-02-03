using System;
using System.Collections.Generic;
using System.Text;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
    public class ChoiceQuestion : Question
    {
        public ChoiceQuestion(string title, string description, string label, List<AnswerPotential> potentials, bool allowNull, string notNullErrorMessage, string notValidErrorMessage)
        {
            Description = description;
            Title = title;
            Id = Guid.NewGuid();
            QuestionType = QuestionType.Select;
            Answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = Id,
                    AllowNull = allowNull,
                    Label = label,
                    AnswerPotentials = potentials,
                    IsConditional = false,
                    NullErrorMessage = notNullErrorMessage,
                    InValidErrorMessage = notValidErrorMessage,
                    Order = 0,
                },
            };
        }
    }
}
