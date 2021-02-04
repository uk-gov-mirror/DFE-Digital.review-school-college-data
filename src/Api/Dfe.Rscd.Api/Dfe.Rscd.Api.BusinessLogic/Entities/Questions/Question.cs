using System;
using System.Collections.Generic;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
	public class Question : IValidatable
    {
        public Question()
        {
            Answer = new Answer();
            Validator = new Validator();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public QuestionType QuestionType { get; set; }
        public bool IsValid()
        {
            return Validator.IsValid();
        }

        public List<string> Validate()
        {
            return Validator.Validate(Answer.Value);
        }

        protected Validator Validator { get; set; }
        public Answer Answer { get; set; }
    }
}

