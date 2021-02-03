using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class PupilNativeLanguageQuestion : ConditionalFurtherQuestion
    {
        public PupilNativeLanguageQuestion(List<AnswerPotential> languages) : base(
            "Pupil's native language",
            "",
            "Select pupil's native language",
            languages,
            false,
            "Enter a valid pupil's native language",
            "",
            "?",
            new AlphabeticalTextQuestion(
                "Enter pupil's native language",
                "",
                "You have selected 'Other'. Please enter the pupil's native language",
                false,
                "Enter pupil's native language",
                "Enter a valid pupil's native language")) { }
    }
}
