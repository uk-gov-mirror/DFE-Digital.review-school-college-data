using System;
using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Common;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Amendments;
using Dfe.Rscd.Api.Domain.Entities.Questions;
using Dfe.Rscd.Api.Services.Rules.RemovePupil.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class RemovePupilAdmittedFollowingPermanentExclusion : Rule
    {
        private readonly IEstablishmentService _establishmentService;
        private readonly int ScrutinyCode = 13;

        public const string ReasonDescription = "Admitted following permanent exclusion from a maintained school";
        public override AmendmentType AmendmentType => AmendmentType.RemovePupil;

        public RemovePupilAdmittedFollowingPermanentExclusion(IEstablishmentService establishmentService)
        {
            _establishmentService = establishmentService;
        }

        public override List<Question> GetQuestions(Amendment amendment)
        {
            var laestabQuestion = new LaestabNumberQuestion(
                CustomValidator,
                Content.LaestabNumberQuestion_Title,
                Content.LaestabNumberQuestion_Label,
                Content.LaestabNumberQuestion_NullErrorMessage);
           
            var pupilExclusionDateQuestion = new PupilExclusionDateQuestion();

            var explainQuestion = new ExplainYourRequestQuestion(string.Empty, "Please explain your request to remove this Pupil");

            var evidenceQuestion =
                new EvidenceUploadQuestion(Content.RemovePupilAdmittedFollowingPermanentExclusionEvidence_HTML);

            return new List<Question> { laestabQuestion, pupilExclusionDateQuestion , explainQuestion, evidenceQuestion };
        }

        private bool CustomValidator(string arg)
        {
            if (!string.IsNullOrEmpty(arg))
            {
                return _establishmentService.DoesSchoolExist(arg); 
            }

            return false;
        }

        protected override AmendmentOutcome ApplyRule(Amendment amendment)
        {
            var laestabAnswer = GetAnswer(amendment, nameof(LaestabNumberQuestion));

            var establishment = _establishmentService
                .GetByDFESNumber(amendment.CheckingWindow, laestabAnswer.Value);

            var pupilExclusionDate = GetAnswer(amendment, nameof(PupilExclusionDateQuestion));
            
            var evidenceUploadAnswer = GetAnswer(amendment, nameof(EvidenceUploadQuestion));

            amendment.EvidenceStatus = string.IsNullOrEmpty(evidenceUploadAnswer.Value) || evidenceUploadAnswer.Value == "0"
                ? EvidenceStatus.Later : EvidenceStatus.Now;

            if (!IsStateFundedOrFeCollege(establishment.InstitutionTypeNumber??0))
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "The excluding school must be a recognized maintained school to meet DCSF criteria for 'pupils admitted following permanent exclusion from a maintained school'")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFollowingPermanentExclusion,
                    ReasonDescription = ReasonDescription
                };
            }

            if (pupilExclusionDate.Value.ToDateTimeWhenSureNotNull() > amendment.Pupil.AdmissionDate)
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "The exclusion date is after the date of admission to your school")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFollowingPermanentExclusion,
                    ReasonDescription = ReasonDescription
                };
            }

            if (pupilExclusionDate.Value.ToDateTimeWhenSureNotNull() < 
                new DateTime(DateTime.Today.AddYears(-2).Year, 8, 01))
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, $"Exclusion Date before 1st September {DateTime.Today.AddYears(-2).Year} - does not meet DCSF criteria for 'pupils admitted following permanent exclusion from a maintained school'")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFollowingPermanentExclusion,
                    ReasonDescription = ReasonDescription
                };
            }

            if (!amendment.Pupil.PortlandStudentID.HasValue)
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "Request to add an unlisted pupil who was admitted following permanent exclusion from a maintained school.  Addition will be reviewed. Please add any missing attainment and send the evidence requested.")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFollowingPermanentExclusion,
                    ReasonDescription = ReasonDescription
                };
            }

            return new AmendmentOutcome(OutcomeStatus.AutoAccept)
            {
                ScrutinyStatusCode = ScrutinyCode.ToString(),
                ReasonId = (int) AmendmentReasonCode.AdmittedFollowingPermanentExclusion,
                ReasonDescription = ReasonDescription
            };
        }

        private bool IsStateFundedOrFeCollege(int schoolType)
        {
            return School.StateFundedSchools.Contains(schoolType) || schoolType == 31;
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

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_LAESTABNumber, 
                    GetAnswer(amendment, nameof(LaestabNumberQuestion)).Value);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ExclusionDate, 
                    GetAnswer(amendment, nameof(PupilExclusionDateQuestion)).Value);
                
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_Detail,
                    GetAnswer(amendment, nameof(ExplainYourRequestQuestion)).Value);
            }
        }

        public override int AmendmentReason => (int)AmendmentReasonCode.AdmittedFollowingPermanentExclusion;
    }
}
