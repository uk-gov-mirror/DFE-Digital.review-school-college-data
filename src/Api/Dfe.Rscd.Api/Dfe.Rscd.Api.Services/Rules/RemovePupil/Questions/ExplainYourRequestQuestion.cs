using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class ExplainYourRequestQuestion : ExplainQuestion
    {
        public ExplainYourRequestQuestion(string reason) : base(nameof(ExplainYourRequestQuestion), Content.ExplainYourRequestQuestion_Title, 
            reason, 
            new Validator
            {
                AllowNull = false,
                ValidatorType = ValidatorType.MaxCharacters,
                InValidErrorMessage = Content.ExplainYourRequestQuestion_InvalidErrorMessage,
                ValidatorCompareValue = "500",
                NullErrorMessage = Content.ExplainYourRequestQuestion_NullErrorMessage
            })
        {
            
        }
    }
}