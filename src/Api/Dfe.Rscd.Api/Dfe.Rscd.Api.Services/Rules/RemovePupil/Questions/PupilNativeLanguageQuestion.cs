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
                    Content.PupilNativeLanguageQuestion_Select_Title,
                    Content.PupilNativeLanguageQuestion_Select_Label, languages, new Validator
                    {
                        AllowNull = false,
                        InValidErrorMessage = string.Empty,
                        NullErrorMessage = Content.PupilNativeLanguageQuestion_Select_NullErrorMessage,
                        ValidatorType = ValidatorType.None
                    });

            SetupNonConditionalQuestion(nonConditionalQuestion);

            var conditionalQuestion = new StringQuestion(
                $"{nameof(PupilNativeLanguageQuestion)}.Other",
                Content.PupilNativeLanguageQuestion_Other_Title,
                Content.PupilNativeLanguageQuestion_Other_Label, 
                new Validator
                {
                    InValidErrorMessage = Content.PupilNativeLanguageQuestion_Other_InvalidErrorMessage,
                    NullErrorMessage = Content.PupilNativeLanguageQuestion_Other_NullErrorMessage,
                    ValidatorType = ValidatorType.AlphabeticalIncludingSpecialChars,
                    AllowNull = false,
                });
        
            SetupConditionalQuestion(conditionalQuestion, "Other");
        }
    }
}
