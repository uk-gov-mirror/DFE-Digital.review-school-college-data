using System;
using System.Collections.Generic;
using System.Text;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
    public class BooleanQuestion : Question
    {
        public BooleanQuestion(string id, string title, string label, Validator validator)
        {
            Title = title;
            Id = id;
            QuestionType = QuestionType.Boolean;
            Validator = validator;
            Answer = new Answer
            {
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
                IsConditional = false
            };
        }
    }
}
