using System.Collections.Generic;
using System.Linq;
using System.Resources;
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

            if (pupilDateOffRoleQuestion.HasAnswer(amendment) && 
                pupilDateOffRoleQuestion.GetAnswer(amendment).Value.ToDateTimeWhenSureNotNull() < _config.CensusDate.ToDateTimeWhenSureNotNull())
            {
                var explainQuestion = new ExplainYourRequestQuestion("The date off roll is before the January census but this pupil was recorded on your January census");
                questions.Add(explainQuestion);
            }
            
            var evidenceQuestion = new EvidenceUploadQuestion(_evidenceHelpDeskText);
            questions.Add(evidenceQuestion);

            return questions;
        }

        protected override List<ValidatedAnswer> GetValidatedAnswers(Amendment amendment)
        {
            var answers = new List<ValidatedAnswer>();
            var questions = GetQuestions(amendment);

            var dateOffRollQuestion = questions.Single(x => x.Id == nameof(PupilDateOffRollQuestion));
            answers.Add(dateOffRollQuestion.GetAnswer(amendment));

            if (questions.Any(x => x.Id == nameof(ExplainYourRequestQuestion)))
            {
                var explainRequestQuestion = questions.Single(x => x.Id == nameof(ExplainYourRequestQuestion));
                answers.Add(explainRequestQuestion.GetAnswer(amendment));
            }
            
            var evidenceQuestion = questions.Single(x => x.Id == nameof(EvidenceUploadQuestion));
            answers.Add(evidenceQuestion.GetAnswer(amendment));

            return answers;
        }

        protected override AmendmentOutcome ApplyRule(Amendment amendment, List<ValidatedAnswer> answers)
        {
            var dateOffRoll = answers.Single(x => x.QuestionId == nameof(PupilDateOffRollQuestion));
            var evidenceUploadQuestion = answers.Single(x => x.QuestionId == nameof(EvidenceUploadQuestion));

            amendment.EvidenceStatus = string.IsNullOrEmpty(evidenceUploadQuestion.Value) || evidenceUploadQuestion.Value == "0"
                ? EvidenceStatus.Later : EvidenceStatus.Now;

            if (dateOffRoll.Value.ToDateTimeWhenSureNotNull() >= _config.CensusDate.ToDateTimeWhenSureNotNull())
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "Date Pupil Removed from Roll should older than Annual School Census Date")
                {
                    ScrutinyStatusCode = string.Empty,
                    ReasonId = (int)AmendmentReasonCode.Other,
                    ReasonDescription = "Other",
                    SubReason = ReasonDescription
                };
            }
            
            return new AmendmentOutcome(OutcomeStatus.AwatingDfeReview, ReasonDescription)
            {
                ScrutinyStatusCode = string.Empty,
                ReasonId = (int)AmendmentReasonCode.Other,
                ReasonDescription = "Other",
                SubReason = ReasonDescription
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
                
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_SubReasonDescription,
                    amendmentOutcome.SubReason);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_OutcomeDescription,
                    amendmentOutcome.OutcomeDescription);
                
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_DateOffRoll,
                    GetAnswer(answers, nameof(PupilDateOffRollQuestion)).Value);

                if (HasAnswer(answers, nameof(ExplainYourRequestQuestion)))
                {
                    amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_Detail,
                        GetAnswer(answers, nameof(ExplainYourRequestQuestion)).Value);
                }
            }
        }

        public string ReasonDescription => "Other - Elective home education";

        public override int AmendmentReason => (int)AmendmentReasonCode.OtherEHE;
    }
}