using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public abstract class Rule : IRule
    {
        public abstract AmendmentType AmendmentType { get; }

        public abstract List<Question> GetQuestions();

        public virtual AmendmentOutcome Apply(Amendment amendment)
        {
            var questions = GetQuestions();

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

            if (amendment.Answers == null || amendment.Answers.Count == 0)
            {
                return new AmendmentOutcome(questions);
            }

            if (errorMessages.Count > 0 && amendment.Answers != null && amendment.Answers.Count > 0)
            {
                return new AmendmentOutcome(questions, errorMessages);
            }
            
            AmendmentOutcome amendmentOutcome = ApplyRule(amendment);

            ApplyOutcomeToAmendment(amendment, amendmentOutcome);

            return amendmentOutcome;
        }

        protected abstract AmendmentOutcome ApplyRule(Amendment amendment);

        protected abstract void ApplyOutcomeToAmendment(Amendment amendment, AmendmentOutcome amendmentOutcome);

        public abstract int AmendmentReason { get; }
    }
}