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
                    Content.CountryPupilLeftEnglandFor_Select_Title,
                    Content.CountryPupilLeftEnglandFor_Select_Label,
                    countries, new Validator
                    {
                        AllowNull = false,
                        InValidErrorMessage = string.Empty,
                        NullErrorMessage = Content.CountryPupilLeftEnglandFor_Select_NullErrorMessage,
                        ValidatorType = ValidatorType.None
                    });

            SetupNonConditionalQuestion(nonConditionalQuestion);

            var conditionalQuestion = new StringQuestion(
                $"{nameof(CountryPupilLeftEnglandFor)}.Other",
                Content.CountryPupilLeftEnglandFor_Other_Title,
                Content.CountryPupilLeftEnglandFor_Other_Label, new Validator
                {
                    InValidErrorMessage = Content.CountryPupilLeftEnglandFor_Other_InvalidErrorMessage,
                    NullErrorMessage = Content.CountryPupilLeftEnglandFor_Other_NullErrorMessage,
                    ValidatorType = ValidatorType.AlphabeticalIncludingSpecialChars,
                    AllowNull = false,
                });

            SetupConditionalQuestion(conditionalQuestion, "Other");
        }
    }
}
