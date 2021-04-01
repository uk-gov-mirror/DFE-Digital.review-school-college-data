using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class PupilDateOnRollQuestion : DateTimeQuestion
    {
        public PupilDateOnRollQuestion() :
            base(nameof(PupilDateOnRollQuestion),
                Content.PupilDateOnRollQuestion_Title,
                Content.PupilDateOnRollQuestion_Label,
                new Validator
                {
                    AllowNull = false,
                    NullErrorMessage = Content.PupilDateOnRollQuestion_NullErrorMessage,
                    ValidatorType = ValidatorType.DateTimeHistorical,
                    InValidErrorMessage = Content.PupilDateOnRollQuestion_InvalidErrorMessage
                })
        {
            
        }
    }
}