using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class PupilCountryQuestion : ConditionalFurtherQuestion
    {
        public PupilCountryQuestion(List<AnswerPotential> countries) : base(
            "Pupil's originating country",
            "",
            "Select pupil's originating country",
            countries,
            false,
            "Select pupil's originating country",
            "",
            "?",
            new AlphabeticalTextQuestion(
                "Enter pupil's originating country",
                "",
                "You have selected 'Other'. Please enter the pupil's originating country.",
                false,
                "Enter pupil's native language",
                "Enter a valid pupil's native language")) { }
    }
}
