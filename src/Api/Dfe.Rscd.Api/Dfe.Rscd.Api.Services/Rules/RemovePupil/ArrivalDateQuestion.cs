using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class ArrivalDateQuestion : NestedChoiceQuestion
    {
        public ArrivalDateQuestion()
        {
            Setup();
        }

        public void Setup()
        {
            var parentQuestion = new SelectQuestion(
                "Do you know pupil's date of arrival to UK?",
                "Select one",
                new List<AnswerPotential>
                {
                    new AnswerPotential {Value = "1", Description = "Enter pupil's date of arrival to UK"},
                    new AnswerPotential {Value = "2", Description = "Do not know pupil's date of arrival to UK"}
                }, new Validator
                {
                    AllowNull = false,
                    InValidErrorMessage = string.Empty,
                    NullErrorMessage = "Select one",
                    ValidatorType = ValidatorType.None
                });

            SetupParentQuestion(parentQuestion);

            var nestedQuestion = new DateTimeQuestion(
                "Enter a date of arrival to UK",
                "For example, 12 11 2007", new Validator
                {
                    InValidErrorMessage = "Enter a valid date of arrival to UK",
                    NullErrorMessage = "Enter a date of arrival to UK",
                    ValidatorType = ValidatorType.DateTimeHistorical,
                    AllowNull = false,
                });

            SetupNestedQuestion(nestedQuestion, "1");
        }
    }
}
