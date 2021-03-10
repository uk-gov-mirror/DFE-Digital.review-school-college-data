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
                    Content.EvidenceUploadQuestion_Boolean_Title,
                    Content.EvidenceUploadQuestion_Boolean_Label,
                    Content.EvidenceUploadQuestion_Boolean_Yes_Description,
                    Content.EvidenceUploadQuestion_Boolean_No_Description,
                    new Validator
                    {
                        AllowNull = false,
                        InValidErrorMessage = string.Empty,
                        NullErrorMessage = Content.EvidenceUploadQuestion_Boolean_NullErrorMessage,
                        ValidatorType = ValidatorType.None
                    });

            SetupNonConditionalQuestion(nonConditionalQuestion);

            var conditionalQuestion = new EvidenceQuestion(
                $"{nameof(EvidenceUploadQuestion)}.1",
                Content.EvidenceUploadQuestion_Upload_Title,
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