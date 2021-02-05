using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class PupilNativeLanguageQuestion : ConditionalFurtherQuestion
    {
        public PupilNativeLanguageQuestion(List<AnswerPotential> languages)
        {
            Setup(languages);
        }

        private void Setup(List<AnswerPotential> languages)
        {
            var nonConditionalQuestion =
                new SelectQuestion(
                    nameof(PupilNativeLanguageQuestion),
                    "Pupil's native language",
                    "Select pupil's native language", languages, new Validator
                    {
                        AllowNull = false,
                        InValidErrorMessage = string.Empty,
                        NullErrorMessage = "Select pupil's native language",
                        ValidatorType = ValidatorType.None
                    });

            SetupNonConditionalQuestion(nonConditionalQuestion);

            var conditionalQuestion = new StringQuestion(
                $"{nameof(PupilNativeLanguageQuestion)}.Other",
                "Enter pupil's native language",
                "You have selected 'Other'. Please enter the pupil's native language", 
                new Validator
                {
                    InValidErrorMessage = "Enter a valid pupil's native language",
                    NullErrorMessage = "Enter pupil's native language",
                    ValidatorType = ValidatorType.AlphabeticalIncludingSpecialChars,
                    AllowNull = false,
                });
        
            SetupConditionalQuestion(conditionalQuestion, "Other");
        }
    }
}
