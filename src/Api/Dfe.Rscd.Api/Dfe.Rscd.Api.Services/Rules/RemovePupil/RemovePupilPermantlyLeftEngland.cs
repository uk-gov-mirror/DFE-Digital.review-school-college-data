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

        public RemovePupilPermanentlyLeftEngland(IDataService dataService, IAllocationYearConfig config)
        {
            _dataService = dataService;
            _config = config;
        }
        
        public override AmendmentType AmendmentType => AmendmentType.RemovePupil;

        public override List<Question> GetQuestions()
        {
            var countries = _dataService.GetAnswerPotentials(nameof(PupilCountryQuestion));
            
            var countryQuestion = new CountryPupilLeftEnglandFor(countries.ToList());
            var pupilDateOffRoleQuestion = new PupilDateOffRollQuestion();
            var explainQuestion = new ExplainYourRequestQuestion("The date off roll is before the January census but this pupil was recorded on your January census");
            var evidenceQuestion = new EvidenceUploadQuestion();

            return new List<Question> { countryQuestion, pupilDateOffRoleQuestion, explainQuestion, evidenceQuestion };
        }

        protected override List<ValidatedAnswer> GetValidatedAnswers(List<UserAnswer> userAnswers)
        {
            var questions = GetQuestions();

            var countryAnswer = questions.Single(x => x.Id == nameof(CountryPupilLeftEnglandFor));
            var pupilDateOffRoleAnswer = questions.Single(x => x.Id == nameof(PupilDateOffRollQuestion));
            var explainRequestAnswer = questions.Single(x => x.Id == nameof(ExplainYourRequestQuestion));
            var evidenceAnswer = questions.Single(x => x.Id == nameof(EvidenceUploadQuestion));
            
            return new List<ValidatedAnswer>
            {
                countryAnswer.GetAnswer(userAnswers),
                pupilDateOffRoleAnswer.GetAnswer(userAnswers),
                explainRequestAnswer.GetAnswer(userAnswers),
                evidenceAnswer.GetAnswer(userAnswers)
            };
        }

        protected override AmendmentOutcome ApplyRule(Amendment amendment, List<ValidatedAnswer> answers)
        {
            var dateOffRoll = answers.Single(x => x.QuestionId == nameof(PupilDateOffRollQuestion));
            var explainRequestAnswer = answers.Single(x => x.QuestionId == nameof(ExplainYourRequestQuestion));
            var evidenceUploadQuestion = answers.Single(x => x.QuestionId == nameof(EvidenceUploadQuestion));

            amendment.EvidenceStatus = string.IsNullOrEmpty(evidenceUploadQuestion.Value) || evidenceUploadQuestion.Value == "0"
                ? EvidenceStatus.Later : EvidenceStatus.Now;

            if (dateOffRoll.Value.ToDateTimeWhenSureNotNull() <
                _config.CensusDate.ToDateTimeWhenSureNotNull() && !string.IsNullOrEmpty(explainRequestAnswer.Value))
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

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_CountryLeftEnglandFor,
                    GetAnswer(answers, nameof(CountryPupilLeftEnglandFor)).Value);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_DateOffRoll,
                    GetAnswer(answers, nameof(PupilDateOffRollQuestion)).Value);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_Detail,
                    GetAnswer(answers, nameof(ExplainYourRequestQuestion)).Value);
            }
        }

        public override int AmendmentReason => (int)AmendmentReasonCode.PermanentlyLeftEngland;
    }
}