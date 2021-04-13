using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Common;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Amendments;
using Dfe.Rscd.Api.Domain.Entities.Questions;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class RemovePupilOtherElectiveHomeEducationRule : Rule
    {
        private readonly IAllocationYearConfig _config;
        
        private string _evidenceHelpDeskText => Content.RemovePupilOtherElectiveHomeEducationRule_HTML;

        public RemovePupilOtherElectiveHomeEducationRule(IAllocationYearConfig config)
        {
            _config = config;
        }
        
        public override AmendmentType AmendmentType => AmendmentType.RemovePupil;
        public override List<Question> GetQuestions(Amendment amendment)
        {
            var questions = new List<Question>();
            
            var pupilDateOffRoleQuestion = new PupilDateOffRollQuestion();
            questions.Add(pupilDateOffRoleQuestion);

            if (GetAnswer(amendment, nameof(PupilDateOffRollQuestion))?.Value.ToDateTimeWhenSureNotNull() < _config.CensusDate.ToDateTimeWhenSureNotNull())
            {
                var explainQuestion = new ExplainYourRequestQuestion("The date off roll is before the January census but this pupil was recorded on your January census");
                questions.Add(explainQuestion);
            }
            
            var evidenceQuestion = new EvidenceUploadQuestion(_evidenceHelpDeskText);
            questions.Add(evidenceQuestion);

            return questions;
        }

        protected override AmendmentOutcome ApplyRule(Amendment amendment)
        {
            var dateOffRoll = GetAnswer(amendment, nameof(PupilDateOffRollQuestion));
            var evidenceUploadQuestion = GetAnswer(amendment, nameof(EvidenceUploadQuestion));

            amendment.EvidenceStatus = string.IsNullOrEmpty(evidenceUploadQuestion.Value) || evidenceUploadQuestion.Value == "0"
                ? EvidenceStatus.Later : EvidenceStatus.Now;

            return new AmendmentOutcome(OutcomeStatus.AwatingDfeReview, null)
            {
                ScrutinyStatusCode = string.Empty,
                ReasonId = (int)AmendmentReasonCode.Other,
                ReasonDescription = "Other",
                SubReason = ReasonDescription
            };
        }

        protected override void ApplyOutcomeToAmendment(Amendment amendment, AmendmentOutcome amendmentOutcome)
        {
            if (amendmentOutcome.IsComplete && amendmentOutcome.FurtherQuestions == null)
            {
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ReasonDescription,
                    amendmentOutcome.ReasonDescription);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ReasonCode,
                    amendmentOutcome.ReasonId);
                
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_SubReasonDescription,
                    amendmentOutcome.SubReason);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_OutcomeDescription,
                    amendmentOutcome.OutcomeDescription);
                
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_DateOffRoll,
                    GetAnswer(amendment, nameof(PupilDateOffRollQuestion)).Value);

                if (HasAnswer(amendment, nameof(ExplainYourRequestQuestion)))
                {
                    amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_Detail,
                        GetAnswer(amendment, nameof(ExplainYourRequestQuestion)).Value);
                }
            }
        }

        public string ReasonDescription => "Other - Elective home education";

        public override int AmendmentReason => (int)AmendmentReasonCode.OtherEHE;
    }
}