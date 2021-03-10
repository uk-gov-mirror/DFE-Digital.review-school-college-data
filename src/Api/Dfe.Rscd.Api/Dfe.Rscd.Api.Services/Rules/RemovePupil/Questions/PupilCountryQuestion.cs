using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class PupilCountryQuestion : ConditionalFurtherQuestion
    {
        public PupilCountryQuestion(List<AnswerPotential> countries)
        {
            Setup(countries);
        }

        private void Setup(List<AnswerPotential> countries)
        {
            var nonConditionalQuestion =
                new SelectQuestion(
                    nameof(PupilCountryQuestion),
                    Content.PupilCountryQuestion_Select_Title,
                    Content.PupilCountryQuestion_Select_Label,
                    countries, new Validator
                    {
                        AllowNull = false,
                        InValidErrorMessage = string.Empty,
                        NullErrorMessage = Content.PupilCountryQuestion_Select_NullErrorMessage,
                        ValidatorType = ValidatorType.None
                    });

            SetupNonConditionalQuestion(nonConditionalQuestion);

            var conditionalQuestion = new StringQuestion(
                $"{nameof(PupilCountryQuestion)}.Other",
                Content.PupilCountryQuestion_Other_Title,
                Content.PupilCountryQuestion_Other_Label, new Validator
                {
                    InValidErrorMessage = Content.PupilCountryQuestion_Other_InvalidErrorMessage,
                    NullErrorMessage = Content.PupilCountryQuestion_Other_NullErrorMessage,
                    ValidatorType = ValidatorType.AlphabeticalIncludingSpecialChars,
                    AllowNull = false,
                });

            SetupConditionalQuestion(conditionalQuestion, "Other");
        }
    }
}
