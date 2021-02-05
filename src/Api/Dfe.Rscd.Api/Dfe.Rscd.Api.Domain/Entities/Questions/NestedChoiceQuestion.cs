using System;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
    public abstract class NestedChoiceQuestion : Question 
    {
        protected virtual void SetupParentQuestion(Question question)
        {
            Title = question.Title;
            Id = question.Id;
            QuestionType = question.QuestionType;
            Answer = question.Answer;
            Answer.HasConditional = true;
            Validator = question.GetValidator();
        }

        protected virtual void SetupNestedQuestion(Question question, string conditionalValue)
        {
            Answer.ConditionalQuestion = question;
            Answer.ConditionalQuestion.Answer.IsConditional = true;
            Answer.ConditionalValue = conditionalValue;
        }
    }
}
