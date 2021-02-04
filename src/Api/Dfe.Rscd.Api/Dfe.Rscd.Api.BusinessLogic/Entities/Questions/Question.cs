using System;
using System.Collections.Generic;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
	public class Question : IValidatable
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public QuestionType QuestionType { get; set; }
        public bool IsValid()
        {
            if (Validator == null)
                throw new ArgumentNullException($"Validator not specified");

            return Validator.IsValid();
        }

        public List<string> Validate(string answer)
        {
            if (Validator == null)
                throw new ArgumentNullException($"Validator not specified");

            return Validator.Validate(answer);
        }

        protected Validator Validator { get; set; }

        public Validator GetValidator()
        {
            return Validator;
        }

        public Answer Answer { get; set; }
    }
}

