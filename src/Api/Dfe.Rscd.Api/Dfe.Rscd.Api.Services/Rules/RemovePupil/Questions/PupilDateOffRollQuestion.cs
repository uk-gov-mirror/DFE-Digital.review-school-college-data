using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class PupilDateOffRollQuestion : DateTimeQuestion
    {
        public PupilDateOffRollQuestion() : 
            base(nameof(PupilDateOffRollQuestion), 
                "Pupil's date off roll",
                "Enter the date that the pupil was off roll",
                new Validator
                {
                    AllowNull = false,
                    NullErrorMessage = "Enter pupil's date off roll",
                    ValidatorType = ValidatorType.DateTimeHistorical,
                    InValidErrorMessage = "Enter a valid pupil's date off roll"
                })
        {
            
        }
    }
}
