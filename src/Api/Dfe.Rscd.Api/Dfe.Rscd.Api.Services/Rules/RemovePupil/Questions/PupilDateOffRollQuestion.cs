using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class PupilDateOffRollQuestion : DateTimeQuestion
    {
        public PupilDateOffRollQuestion() : 
            base(nameof(PupilDateOffRollQuestion), 
                Content.PupilDateOffRollQuestion_Title,
                Content.PupilDateOffRollQuestion_Label,
                new Validator
                {
                    AllowNull = false,
                    NullErrorMessage = Content.PupilDateOffRollQuestion_NullErrorMessage,
                    ValidatorType = ValidatorType.DateTimeHistorical,
                    InValidErrorMessage = Content.PupilDateOffRollQuestion_InvalidErrorMessage
                })
        {
            
        }
    }
}
