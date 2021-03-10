using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class ArrivalDateQuestion : NullableDateQuestion
    {
        public ArrivalDateQuestion()
        {
            Setup();
        }

        public void Setup()
        {
            SetupParentYesNo(
                nameof(ArrivalDateQuestion),
                Content.ArrivalDateQuestion_Title,
                Content.ArrivalDateQuestion_Label, new List<AnswerPotential>
                {
                    new AnswerPotential {Value = "1", Description = Content.ArrivalDateQuestion_Yes_Description},
                    new AnswerPotential {Value = "2", Description = Content.ArrivalDateQuestion_No_Description}
                }, new Validator
                {
                    InValidErrorMessage = Content.ArrivalDateQuestion_InvalidErrorMessage,
                    NullErrorMessage = Content.ArrivalDateQuestion_NullErrorMessage,
                    ValidatorType = ValidatorType.DateTimeHistorical,
                    AllowNull = true,
                });

            var dateTimeQuestion = new DateTimeQuestion(
                nameof(ArrivalDateQuestion),
                Content.ArrivalDateQuestion_Date_Title,
                Content.ArrivalDateQuestion_Date_Label, new Validator{ ValidatorType = ValidatorType.None });

            SetupDateQuestion(dateTimeQuestion);
        }
    }
}
