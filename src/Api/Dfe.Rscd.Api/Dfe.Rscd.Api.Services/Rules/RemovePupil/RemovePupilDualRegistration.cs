using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Amendments;
using Dfe.Rscd.Api.Domain.Entities.Questions;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;
using Dfe.Rscd.Api.Services;
using Dfe.Rscd.Api.Services.Rules;
using Dfe.Rscd.Api.Services.Rules.RemovePupil.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class RemovePupilDualRegistration : Rule
    {
        private readonly IEstablishmentService _establishmentService;
        public string ReasonDescription = "Dual registration";
        public const int ScrutinyCode = 4;
        public override int AmendmentReason => (int) AmendmentReasonCode.DualRegistration;

        private bool CustomLaEstabValidator(string arg)
        {
            if (!string.IsNullOrEmpty(arg))
            {
                return _establishmentService.DoesSchoolExist(arg);
            }

            return false;
        }

        public override AmendmentType AmendmentType => AmendmentType.RemovePupil;

        private readonly IAllocationYearConfig _config;

        public RemovePupilDualRegistration(IEstablishmentService establishmentService)
        {
            _establishmentService = establishmentService;
        }

        public override List<Question> GetQuestions(Amendment amendment)
        {
            var laestabQuestion = new LaestabNumberQuestion(
                CustomLaEstabValidator,
                Content.RemovePupilDualRegLAESTABTitle,
                Content.RemovePupilDualRegLAESTABLabel,
                Content.RemovePupilDualRegLAESTABError);
            
            var explainQuestion =
                new ExplainYourRequestQuestion(Content.RemovePupilDualRegExplainDetailsLabel, "Please explain your request");
            var evidenceQuestion =
                new EvidenceUploadQuestion(Content.RemovePupilDualRegEvidence);

            return new List<Question>
            {
                laestabQuestion, explainQuestion, evidenceQuestion
            };
        }

        protected override AmendmentOutcome ApplyRule(Amendment amendment)
        {
            var laestabAnswer = GetAnswer(amendment, nameof(LaestabNumberQuestion));

            var establishment = _establishmentService
                .GetByDFESNumber(amendment.CheckingWindow, laestabAnswer.Value);

            var evidenceUploadAnswer = GetAnswer(amendment, nameof(EvidenceUploadQuestion));

            amendment.EvidenceStatus =
                string.IsNullOrEmpty(evidenceUploadAnswer.Value) || evidenceUploadAnswer.Value == "0"
                    ? EvidenceStatus.Later
                    : EvidenceStatus.Now;

            return new AmendmentOutcome(OutcomeStatus.AwatingDfeReview, null)
            {
                ScrutinyStatusCode = ScrutinyCode.ToString(),
                ReasonId = (int) AmendmentReasonCode.DualRegistration,
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

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_PreviousLAESTABNumber,
                    GetAnswer(amendment, nameof(LaestabNumberQuestion)).Value);

                if (HasAnswer(amendment, nameof(ExplainYourRequestQuestion)))
                {
                    amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_Detail,
                        GetAnswer(amendment, nameof(ExplainYourRequestQuestion)).Value);
                }
            }

        }
    }
}