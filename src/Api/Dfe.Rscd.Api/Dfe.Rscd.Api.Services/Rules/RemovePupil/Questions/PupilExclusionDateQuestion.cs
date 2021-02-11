using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class PupilExclusionDateQuestion : DateTimeQuestion
    {
        public PupilExclusionDateQuestion() : 
            base(nameof(PupilExclusionDateQuestion), 
                "Pupil's exclusion date",
                "Enter the date that the pupil was excluded from the previous school",
                new Validator
                {
                    AllowNull = false,
                    NullErrorMessage = "Enter pupil's exclusion date",
                    ValidatorType = ValidatorType.DateTimeHistorical,
                    InValidErrorMessage = "Enter a valid pupil's exclusion date"
                })
        {
            
        }
    }
}
