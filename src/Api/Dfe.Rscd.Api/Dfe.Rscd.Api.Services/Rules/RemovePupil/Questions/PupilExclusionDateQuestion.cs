using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class PupilExclusionDateQuestion : DateTimeQuestion
    {
        public PupilExclusionDateQuestion() : 
            base(nameof(PupilExclusionDateQuestion), 
                Content.PupilExclusionDateQuestion_Title,
                Content.PupilExclusionDateQuestion_Label,
                new Validator
                {
                    AllowNull = false,
                    NullErrorMessage = Content.PupilExclusionDateQuestion_NullErrorMessage,
                    ValidatorType = ValidatorType.DateTimeHistorical,
                    InValidErrorMessage = Content.PupilExclusionDateQuestion_InvalidErrorMessage
                })
        {
            
        }
    }
}
