using System;
using System.Collections.Generic;

namespace Dfe.Rscd.Api.Domain.Entities
{
	public class Question 
	{
        public Question()
        {
            Answers = new List<Answer>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public QuestionType QuestionType { get; set; }
        
        public List<Answer> Answers { get; set; }
    }

    public enum QuestionType
    {
        Select,
        DateTime,
        Number,
        String,
        Boolean,
        NestedConditional,
        ConditionalFurther
    }
}

