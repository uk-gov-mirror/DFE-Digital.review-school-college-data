using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class EvidenceUploadQuestion : ConditionalFurtherQuestion
    {
        public EvidenceUploadQuestion()
        {
            Setup();
        }
        
        private void Setup()
        {
            var helpTextUpload = "Evidence to provide<br/><ul><li>Lorem ipsum</li><li>Lorem ipsum</li><li>Lorem ipsum</li>";
            
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

            nonConditionalQuestion.HelpTextHtml = "Evidence to provide<br/><ul><li>Lorem ipsum</li><li>Lorem ipsum</li><li>Lorem ipsum</li>";

            SetupNonConditionalQuestion(nonConditionalQuestion);

            var conditionalQuestion = new EvidenceQuestion(
                $"{nameof(EvidenceUploadQuestion)}.1",
                "Upload evidence",
                helpTextUpload);

            SetupConditionalQuestion(conditionalQuestion, "1");
        }
    }
}