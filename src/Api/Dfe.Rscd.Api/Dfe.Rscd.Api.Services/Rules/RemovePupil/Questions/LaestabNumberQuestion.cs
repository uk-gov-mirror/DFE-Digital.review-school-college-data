using System;
using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules.RemovePupil.Questions
{
    public class LaestabNumberQuestion : NumberQuestion
    {
        public LaestabNumberQuestion(Func<string, bool> customValidator, 
                string title = nameof(Content.LaestabNumberQuestion_Title),
                string label = nameof(Content.LaestabNumberQuestion_Label),
                string errorMessage = nameof(Content.LaestabNumberQuestion_NullErrorMessage)) 
            : base(nameof(LaestabNumberQuestion), 
                title, 
                label,
                new Validator
                {
                    AllowNull = false,
                    NullErrorMessage = errorMessage,
                    ValidatorType = ValidatorType.LAESTABNumber,
                    ValidatorCompareValue = @"^\d{7}$",
                    InValidErrorMessage = Content.LaestabNumberQuestion_InvalidErrorMessage
                }.SetCustomValidationPredicate(customValidator))
        {
            
        }
    }
}
