using System;
using System.Collections.Generic;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
    public class ChoiceQuestion : Question
    {
        public ChoiceQuestion(string id, string title, string label, List<AnswerPotential> potentials, Validator validator)
        {
            Title = title;
            Id = id;
            QuestionType = QuestionType.Choice;
            Validator = validator;
            Answer = new Answer
            {
                Label = label,
                AnswerPotentials = potentials,
                IsConditional = false
            };
        }
    }
}
