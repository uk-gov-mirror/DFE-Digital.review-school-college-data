using System;
using System.Collections.Generic;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
    public class Validator
    {
        public List<string> ValidationErrors { get; set; }

        public bool AllowNull { get; set; }
        public string NullErrorMessage { get; set; }
        public string InValidErrorMessage { get; set; }
        public ValidatorType ValidatorType { get; set; }
        public string ValidatorCompareValue { get; set; }
        
        public bool IsValid()
        {
            return ValidationErrors == null || ValidationErrors.Count == 0;
        }

        public List<string> Validate(string value)
        {
            var errors = new List<string>();
            if (!AllowNull && string.IsNullOrEmpty(value))
            {
                errors.Add(NullErrorMessage);
                return errors;
            }

            var rules = ValidationRules.GetValidationRules(ValidatorCompareValue);
            if (!rules[ValidatorType](value))
            {
                errors.Add(InValidErrorMessage);
            }

            return errors;
        }

    }
}
