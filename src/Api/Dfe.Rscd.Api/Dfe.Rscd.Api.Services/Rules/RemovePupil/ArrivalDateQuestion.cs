using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class ArrivalDateQuestion : NestedChoiceQuestion
    {
        public ArrivalDateQuestion() : base(
            "Do you know pupil's date of arrival to UK?",
            "",
            "Select one",
            new List<AnswerPotential>
            {
                new AnswerPotential{ Value = "1", Description = "Enter pupil's date of arrival to UK"},
                new AnswerPotential{ Value = "2", Description = "Do not know pupil's date of arrival to UK"}
            },
            false,
            "Select one",
            "",
            "1",
            new HistoricalDateTimeQuestion("For example, 12 11 2007",
                "",
                "",
                false,
                "Enter a date of arrival to UK",
                "Enter a valid date of arrival to UK")) { }
    }
}
