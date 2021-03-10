using System;
using System.Collections.Generic;
using System.Text;
using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules.RemovePupil.Questions
{
    public class LaestabNumberQuestion : NumberQuestion
    {
        public LaestabNumberQuestion() 
            : base(nameof(LaestabNumberQuestion), 
                Content.LaestabNumberQuestion_Title, 
                Content.LaestabNumberQuestion_Label, 
                new Validator
                {
                    AllowNull = false,
                    NullErrorMessage = Content.LaestabNumberQuestion_NullErrorMessage,
                    ValidatorType = ValidatorType.LAESTABNumber,
                    ValidatorCompareValue = @"^\d{7}$",
                    InValidErrorMessage = Content.LaestabNumberQuestion_InvalidErrorMessage
                })
        {

        }
    }
}
