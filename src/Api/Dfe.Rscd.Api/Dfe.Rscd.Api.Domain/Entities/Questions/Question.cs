using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Entities.Amendments;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
    public class Question : IValidatable
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string HelpTextHtml { get; set; }
        public QuestionType QuestionType { get; set; }

        public Validator Validator { get; set; }

        public Answer Answer { get; set; }

        public bool IsValid()
        {
            if (Validator == null)
                throw new ArgumentNullException("Validator not specified");

            return Validator.IsValid();
        }

        public List<string> Validate(string answer)
        {
            if (Validator == null)
                throw new ArgumentNullException("Validator not specified");

            return Validator.Validate(answer);
        }
    }
}