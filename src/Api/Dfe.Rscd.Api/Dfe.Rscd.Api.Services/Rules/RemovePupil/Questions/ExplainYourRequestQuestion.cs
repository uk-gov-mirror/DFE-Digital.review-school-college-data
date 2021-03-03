using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class ExplainYourRequestQuestion : ExplainQuestion
    {
        public ExplainYourRequestQuestion(string reason) : base(nameof(ExplainYourRequestQuestion), "Explain your request", 
            reason, 
            new Validator
            {
                AllowNull = false,
                ValidatorType = ValidatorType.MaxCharacters,
                InValidErrorMessage = "You have XXXX characters too many",
                ValidatorCompareValue = "5000",
                NullErrorMessage = "Please provide details to explain your request "
            })
        {
            
        }
    }
}