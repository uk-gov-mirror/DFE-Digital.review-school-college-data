using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Common;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Amendments;
using Dfe.Rscd.Api.Domain.Entities.Questions;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class RemovePupilPermanentlyLeftEngland : Rule
    {
        private readonly IDataService _dataService;
        private readonly IAllocationYearConfig _config;
        private const string ReasonDescription = "Permanently Left England";
        private string ScrutinyCode = "3";

        private string _evidenceHelpDeskText => Content.RemovePupilPermanentlyLeftEngland_HTML;

        public RemovePupilPermanentlyLeftEngland(IDataService dataService, IAllocationYearConfig config)
        {
            _dataService = dataService;
            _config = config;
        }
        
        public override AmendmentType AmendmentType => AmendmentType.RemovePupil;

        public override List<Question> GetQuestions(Amendment amendment)
        {
            var questions = new List<Question>();
            
            var countries = _dataService.GetAnswerPotentials(nameof(PupilCountryQuestion));
            var countryQuestion = new CountryPupilLeftEnglandFor(countries.ToList());
            questions.Add(countryQuestion);
            
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

            if (dateOffRoll.Value.ToDateTimeWhenSureNotNull() < _config.CensusDate.ToDateTimeWhenSureNotNull())
            {
                return new AmendmentOutcome(OutcomeStatus.AwatingDfeReview, "Permanently Left England")
                {
                    ScrutinyStatusCode = ScrutinyCode,
                    ReasonId = AmendmentReason,
                    ReasonDescription = ReasonDescription
                };
            }

            if (!amendment.Pupil.PortlandStudentID.HasValue)
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "Request to add an unlisted pupil who has permanently left England. Addition will be reviewed.")
                {
                    ScrutinyStatusCode = ScrutinyCode,
                    ReasonId = AmendmentReason,
                    ReasonDescription = ReasonDescription
                };
            }
            
            return new AmendmentOutcome(OutcomeStatus.AwatingDfeReview, "Emigrated")
            {
                ScrutinyStatusCode = ScrutinyCode,
                ReasonId = AmendmentReason,
                ReasonDescription = ReasonDescription
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

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_OutcomeDescription,
                    amendmentOutcome.OutcomeDescription);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_CountryLeftEnglandFor,
                    GetFlattenedDisplayField(amendment, nameof(CountryPupilLeftEnglandFor)));

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_DateOffRoll,
                    GetAnswer(amendment, nameof(PupilDateOffRollQuestion)).Value);

                if (HasAnswer(amendment, nameof(ExplainYourRequestQuestion)))
                {
                    amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_Detail,
                        GetAnswer(amendment, nameof(ExplainYourRequestQuestion)).Value);
                }
            }
        }

        public override int AmendmentReason => (int)AmendmentReasonCode.PermanentlyLeftEngland;
    }
}