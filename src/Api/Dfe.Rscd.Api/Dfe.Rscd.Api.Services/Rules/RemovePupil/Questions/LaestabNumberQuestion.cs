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
                "LAESTAB of excluded school", 
                "Enter the LAESTAB of the school the pupil was permanently excluded from", 
                new Validator
                {
                    AllowNull = false,
                    NullErrorMessage = "Enter the LAESTAB",
                    ValidatorType = ValidatorType.LAESTABNumber,
                    ValidatorCompareValue = @"^\d{7}$",
                    InValidErrorMessage = "Enter a valid LAESTAB"
                })
        {

        }
    }
}
