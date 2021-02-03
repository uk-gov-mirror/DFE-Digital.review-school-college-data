using System;
using System.Collections.Generic;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class Answer
    {
        public Answer()
        {
            ConditionalQuestion = new Question();
            AnswerPotentials = new List<AnswerPotential>();
        }

        public Guid QuestionId { get; set; }
        public int Order { get; set; }
        public bool IsConditional { get; set; }
        public bool HasConditional { get;set; }
        public string ConditionalValue { get;set; }
        public Question ConditionalQuestion { get; set; }
        public List<AnswerPotential> AnswerPotentials { get; set; }

        public string Label { get;set; }
        public string Value { get; set; }
        public bool AllowNull { get; set; }
        public bool HistoricalDate { get; set; }
        public bool FutureDate { get; set; }
        
        public string NullErrorMessage { get;set; }
        public string InValidErrorMessage { get; set; }
        public string ValidationRegex { get; set; }
    }
}
