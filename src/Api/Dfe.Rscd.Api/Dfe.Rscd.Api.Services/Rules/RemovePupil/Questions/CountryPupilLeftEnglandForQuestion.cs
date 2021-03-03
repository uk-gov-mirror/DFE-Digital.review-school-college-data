using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class CountryPupilLeftEnglandFor : ConditionalFurtherQuestion
    {
        public CountryPupilLeftEnglandFor(List<AnswerPotential> countries)
        {
            Setup(countries);
        }

        private void Setup(List<AnswerPotential> countries)
        {
            var nonConditionalQuestion =
                new SelectQuestion(
                    nameof(CountryPupilLeftEnglandFor),
                    "Pupil's destination country",
                    "Select the country that the pupil left England for",
                    countries, new Validator
                    {
                        AllowNull = false,
                        InValidErrorMessage = string.Empty,
                        NullErrorMessage = "Select the country that the pupil left England for",
                        ValidatorType = ValidatorType.None
                    });

            SetupNonConditionalQuestion(nonConditionalQuestion);

            var conditionalQuestion = new StringQuestion(
                $"{nameof(CountryPupilLeftEnglandFor)}.Other",
                "Enter the country pupil left England for",
                "You have selected 'Other'. Please enter the country that the pupil left England for", new Validator
                {
                    InValidErrorMessage = "Enter a valid country that the pupil left England for",
                    NullErrorMessage = "Enter the country that the pupil left England for",
                    ValidatorType = ValidatorType.AlphabeticalIncludingSpecialChars,
                    AllowNull = false,
                });

            SetupConditionalQuestion(conditionalQuestion, "Other");
        }
    }
}
