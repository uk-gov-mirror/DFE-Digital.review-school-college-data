using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Common;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Questions;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;
using Dfe.Rscd.Api.Services.Rules.RemovePupil.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class RemovePupilDeceased : Rule
    {
        private readonly IAllocationYearConfig _allocationYearConfig;
        private readonly int ScrutinyCode = 4;

        public const string ReasonDescription = "Deceased";
        public override AmendmentType AmendmentType => AmendmentType.RemovePupil;

        public RemovePupilDeceased(IAllocationYearConfig allocationYearConfig)
        {
            _allocationYearConfig = allocationYearConfig;
        }

        public override List<Question> GetQuestions()
        {
            var dateOffRoll = new PupilDateOffRollQuestion();

            return new List<Question> { dateOffRoll };
        }

        protected override List<ValidatedAnswer> GetValidatedAnswers(List<UserAnswer> userAnswers)
        {
            var questions = GetQuestions();

            var dateOffRoll = questions.Single(x => x.Id == nameof(PupilDateOffRollQuestion));
            
            return new List<ValidatedAnswer>
            {
                dateOffRoll.GetAnswer(userAnswers)
            };
        }

        protected override AmendmentOutcome ApplyRule(Amendment amendment, List<ValidatedAnswer> answers)
        {
            var dateOffRoll = answers.Single(x => x.QuestionId == nameof(PupilDateOffRollQuestion));
            
            if (dateOffRoll.Value.ToDateTimeWhenSureNotNull() < _allocationYearConfig.CensusDate.ToDateTimeWhenSureNotNull())
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "Death")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.Deceased,
                    ReasonDescription = ReasonDescription
                };
            }

            return new AmendmentOutcome(OutcomeStatus.AutoAccept)
            {
                ScrutinyStatusCode = ScrutinyCode.ToString(),
                ReasonId = (int) AmendmentReasonCode.Deceased,
                ReasonDescription = ReasonDescription
            };
        }

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

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_DateOffRoll, 
                    GetAnswer(answers, nameof(PupilExclusionDateQuestion)).Value);
            }
        }

        public override int AmendmentReason => (int)AmendmentReasonCode.Deceased;
    }
}
