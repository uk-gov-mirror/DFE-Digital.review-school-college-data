using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Amendments;
using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class EvidenceUploadQuestion : ConditionalFurtherQuestion
    {
        public EvidenceUploadQuestion(string htmlEvidenceRequiredText)
        {
            Setup(htmlEvidenceRequiredText);
        }
        
        private void Setup(string htmlEvidenceRequiredText)
        {
            var nonConditionalQuestion =
                new BooleanQuestion(
                    nameof(EvidenceUploadQuestion),
                    "Are you able to provide evidence now?",
                    "Select one",
                    "I will provide electronic evidence now",
                    "I will submit electronic evidence before 5pm on DD/MM/YYYY",
                    new Validator
                    {
                        AllowNull = false,
                        InValidErrorMessage = string.Empty,
                        NullErrorMessage = "Select one",
                        ValidatorType = ValidatorType.None
                    });

            SetupNonConditionalQuestion(nonConditionalQuestion);

            var conditionalQuestion = new EvidenceQuestion(
                $"{nameof(EvidenceUploadQuestion)}.1",
                "Upload evidence",
                htmlEvidenceRequiredText);

            SetupConditionalQuestion(conditionalQuestion, "1");
        }

        public override ValidatedAnswer GetAnswer(Amendment amendment)
        {
            var userAnswer = amendment.Answers.Single(x => x.QuestionId == Id);
            var answer = GetAnswerPotential(userAnswer.Value);

            if (Answer.HasConditional && (Answer.ConditionalValue == answer.Value || Answer.ConditionalValue == answer.Description))
            {
                userAnswer = amendment.Answers.Single(x => x.QuestionId == Answer.ConditionalQuestion.Id);
                
                return new ValidatedAnswer
                {
                    Value = userAnswer.Value,
                    IsRejected = answer.Reject,
                    QuestionId = Id
                };
            }
            
            return new ValidatedAnswer
            {
                Value = answer.Value,
                IsRejected = answer.Reject,
                QuestionId = Id
            };
        }
    }
}