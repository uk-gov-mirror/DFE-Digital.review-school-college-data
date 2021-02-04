using System;
using System.Collections.Generic;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
    public class SelectQuestion : Question
    {
        public SelectQuestion(string title, string label, List<AnswerPotential> potentials, Validator validator)
        {
            Title = title;
            Id = Guid.NewGuid();
            QuestionType = QuestionType.Select;
            Validator = validator;
            Answer = new Answer
            {
                QuestionId = Id,
                Label = label,
                AnswerPotentials = potentials,
                IsConditional = false
            };
        }
    }
}
