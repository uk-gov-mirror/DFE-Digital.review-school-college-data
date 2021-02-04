using System;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
    public abstract class ConditionalFurtherQuestion : Question
    {
        protected virtual void SetupNonConditionalQuestion(Question question)
        {
            Title = question.Title;
            Id = Guid.NewGuid();
            QuestionType = question.QuestionType;
            Answer = question.Answer;
            Answer.HasConditional = true;
        }

        protected virtual void SetupConditionalQuestion(Question question, string conditionalValue)
        {
            Answer.ConditionalQuestion = question;
            Answer.ConditionalQuestion.Answer.IsConditional = true;
            Answer.ConditionalValue = conditionalValue;
        }
    }
}
