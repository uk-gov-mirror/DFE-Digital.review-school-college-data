using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Services.Rules
{
    public abstract class RemovePupilRule : Rule
    {
        public override AmendmentType AmendmentType => AmendmentType.RemovePupil;
        
        protected override void ApplyOutcomeToAmendment(Amendment amendment, AmendmentOutcome amendmentOutcome, List<ValidatedAnswer> answers)
        {
            if (amendmentOutcome.IsComplete && amendmentOutcome.FurtherQuestions == null)
            {
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ReasonDescription,
                    amendmentOutcome.ReasonDescription);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ReasonCode,
                    amendmentOutcome.ReasonId);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_OutcomeDescription,
                    amendmentOutcome.OutcomeDescription);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_CountryOfOrigin, 
                    GetAnswer(answers, nameof(PupilCountryQuestion)).Value);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_NativeLanguage, 
                    GetAnswer(answers, nameof(PupilNativeLanguageQuestion)).Value);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_DateOfArrivalUk, 
                    GetAnswer(answers, nameof(ArrivalDateQuestion)).Value);
            }
        }
    }
}