using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Entities.Amendments;
using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public abstract class Rule : IRule
    {
        public abstract AmendmentType AmendmentType { get; }

        public abstract List<Question> GetQuestions(Amendment amendment);

        public virtual AmendmentOutcome Apply(Amendment amendment)
        {
            var questions = GetQuestions(amendment);

            var errorMessages = new Dictionary<string, List<string>>();

            foreach (var question in questions)
            {
                var answer = amendment.Answers.SingleOrDefault(x => x.QuestionId == question.Id);
                if (answer != null)
                {
                    var errors = question.Validate(answer.Value);

                    if (errors.Count > 0)
                    {
                        errorMessages.Add(question.Id, errors);
                    }

                    if (question.Answer.HasConditional)
                    {
                        answer = amendment.Answers.SingleOrDefault(x => x.QuestionId == question.Answer.ConditionalQuestion.Id);
                        if (answer != null)
                        {
                            errors = question.Answer.ConditionalQuestion.Validate(answer.Value);

                            if (errors.Count > 0)
                            {
                                errorMessages.Add(question.Answer.ConditionalQuestion.Id, errors);
                            }
                        }
                    }
                }
            }

            if (errorMessages.Count > 0)
            {
                return new AmendmentOutcome(questions, errorMessages);
            }

            if (amendment.Answers == null || amendment.Answers
                .Where(x => !x.QuestionId.Contains("."))
                .Select(x => x.QuestionId)
                .Distinct()
                .Count() < questions.Count)
            {
                return new AmendmentOutcome(questions);
            }

            AmendmentOutcome amendmentOutcome = ApplyRule(amendment);

            ApplyOutcomeToAmendment(amendment, amendmentOutcome);

            return amendmentOutcome;
        }

        protected abstract AmendmentOutcome ApplyRule(Amendment amendment);

        protected abstract void ApplyOutcomeToAmendment(Amendment amendment, AmendmentOutcome amendmentOutcome);

        public abstract int AmendmentReason { get; }

        protected bool HasAnswer(Amendment amendment, string questionId)
        {
            return amendment.Answers.Any(x => x.QuestionId == questionId);
        }

        protected UserAnswer GetAnswer(Amendment amendment, string questionId)
        {
            if (HasAnswer(amendment, questionId))
            {
                return amendment.Answers.Single(x => x.QuestionId == questionId);
            }

            return null;
        }
        
        protected AnswerPotential GetSelectedAnswerItem(Amendment amendment, string questionId)
        {
            var question = GetQuestions(amendment).Single(x => x.Id == questionId);
            var actualValue = amendment.Answers.Single(x => x.QuestionId == questionId)?.Value;
            
            if (question.Answer.AnswerPotentials != null && question.Answer.AnswerPotentials.Count > 0)
            {
                var answer = question.Answer.AnswerPotentials.SingleOrDefault(x => x.Value == actualValue);
                if (answer != null)
                {
                    return answer;
                }
            }

            return new AnswerPotential {Description = actualValue, Value = actualValue, Reject = false};
        }
        
        protected string GetConditionalValue(Amendment amendment, string questionId)
        {
            var question = GetQuestions(amendment).Single(x => x.Id == questionId);
            
            if (question.Answer.HasConditional)
            {
                var answer = GetAnswer(amendment, question.Answer.ConditionalQuestion.Id);
                if (answer != null)
                {
                    return answer.Value;
                }
            }

            return string.Empty;
        }
        
        protected string GetFlattenedDisplayField(Amendment amendment, string questionId)
        {
            var selectedItem = GetSelectedAnswerItem(amendment, questionId);
            var conditionalValue = GetConditionalValue(amendment, questionId);
            if (string.IsNullOrEmpty(conditionalValue))
            {
                return selectedItem.Description;
            }

            return selectedItem.Description + " - " + conditionalValue;
        }
    }
}