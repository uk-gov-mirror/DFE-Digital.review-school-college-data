using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.BusinessLogic.Entities;
using Dfe.Rscd.Api.Infrastructure.SqlServer.Repositories;

namespace Dfe.Rscd.Api.Services.Rules
{
    public partial class RemovePupilRulesV2 : IRuleSet
    {
        private readonly IDataRepository _dataRepository;

        public RemovePupilRulesV2(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public AmendmentType AmendmentType => AmendmentType.RemovePupil;

        public AdjustmentOutcome Apply(Amendment amendment)
        {
            var furtherPrompts = Context.PromptAnswers;
            AdjustmentOutcome adjOutcomeOut;

            var student = Context.Pupil;
            var inclusionReasonId = Context.InclusionReasonId;
            var promptAnswers = Context.PromptAnswers;
            var dfesNumber = Context.DfesNumber;

            switch (inclusionReasonId)
            {
                //Reason 8
                case ((int)ReasonsForAdjustment.AdmittedFromAbroad):
                    adjOutcomeOut =
                        AdmittedFromAbroad(dfesNumber, student, inclusionReasonId, promptAnswers);
                    break;

                //Reason 10
                case ((int)ReasonsForAdjustment.AdmittedFollowingPermanentExclusionFromMaintainedSchool):
                    adjOutcomeOut =
                        AdmittedFollowingPermanentExclusion(student, inclusionReasonId, promptAnswers);
                    break;

                //Reason 12
                case ((int)ReasonsForAdjustment.Deceased):
                    adjOutcomeOut =
                        ProcessInclusionPromptResponses_Deceased(inclusionReasonId, promptAnswers, student.Id);
                    break;

                //Reason 19
                case ((int)ReasonsForAdjustment.KS4Other):
                    adjOutcomeOut =
                        ProcessInclusionPromptResponses_OtherKS4(student, inclusionReasonId, promptAnswers);
                    break;

                //Reason 54
                case ((int)ReasonsForAdjustment.NotAtEndOfAdvancedStudy):
                    adjOutcomeOut =
                        ProcessInclusionPromptResponses_NotAtEndOfAdvancedStudy(student, inclusionReasonId, promptAnswers);
                    break;

                default:
                    adjOutcomeOut = new AdjustmentOutcome(new List<Prompt>());
                    break;
            }

            return adjOutcomeOut;
        }

        public RuleSetContext Context { get; set; }


        #region Private nethods useful for ProcessInclusionPromptResponses

        internal bool IsPromptAnswerComplete(List<PromptAnswer> promptAnswerList, int promptId)
        {
            bool isPromptAnswerComplete = false;

            if (promptAnswerList.HasPromptAnswer(promptId))
            {
                PromptAnswer answer = promptAnswerList.GetPromptAnswerByPromptId(promptId);
                isPromptAnswerComplete = IsPromptAnswerComplete(answer);
            }
            else
            {
                isPromptAnswerComplete = false;
            }

            return isPromptAnswerComplete;

        }

        internal bool IsPromptAnswerComplete(PromptAnswer answer)
        {
            bool isPromptAnswerComplete = false;
            var prompt = _dataRepository.Get<Prompt>().Single(x => x.PromptID == answer.PromptID);

            bool promptAllowsNulls = prompt.AllowNulls;

            switch (answer.PromptAnswerType)
            {
                case (PromptAnswer.PromptAnswerTypeEnum.Info):
                    if ((answer.PromptAcknowledgeInfoSightAnswer.HasValue && answer.PromptAcknowledgeInfoSightAnswer.Value) || promptAllowsNulls)
                        isPromptAnswerComplete = true;
                    else
                        isPromptAnswerComplete = false;
                    break;

                case (PromptAnswer.PromptAnswerTypeEnum.Text):
                    if (!String.IsNullOrEmpty(answer.PromptStringAnswer) || promptAllowsNulls)
                        isPromptAnswerComplete = true;
                    else
                        isPromptAnswerComplete = false;
                    break;

                case (PromptAnswer.PromptAnswerTypeEnum.Date):
                    if (answer.PromptDateTimeAnswer != null || promptAllowsNulls)
                        isPromptAnswerComplete = true;
                    else
                        isPromptAnswerComplete = false;
                    break;

                case (PromptAnswer.PromptAnswerTypeEnum.Integer):
                    if (answer.PromptIntegerAnswer.HasValue)
                        isPromptAnswerComplete = true;
                    else
                        isPromptAnswerComplete = false;
                    break;

                case (PromptAnswer.PromptAnswerTypeEnum.ListBox):
                    if (!String.IsNullOrEmpty(answer.PromptSelectedValueAnswer) || promptAllowsNulls)
                        isPromptAnswerComplete = true;
                    else
                        isPromptAnswerComplete = false;
                    break;

                case (PromptAnswer.PromptAnswerTypeEnum.YesNo):
                    if (answer.PromptYesNoAnswer.HasValue || promptAllowsNulls)
                        isPromptAnswerComplete = true;
                    else
                        isPromptAnswerComplete = false;
                    break;

                default:
                    isPromptAnswerComplete = false;
                    break;
            }

            return isPromptAnswerComplete;
        }

        private AdjustmentOutcome ProcessSingularFurtherPrompt(int furtherPromptId,
            int studentId,
            int? inclusionReasonId,
            List<PromptAnswer> answers,
            int scrutinyReasonId,
            int? rejectionReasonCode,
            string scrutinyStatusCode,
            string completionMessage)
        {
            var prompt = _dataRepository.Get<Prompt>().Single(x => x.PromptID == furtherPromptId);

            if (!answers.HasPromptAnswer(furtherPromptId))
            {
                List<Prompt> furtherPrompts = new List<Prompt> { prompt };
                return new AdjustmentOutcome(furtherPrompts);
            }

            if (answers.HasPromptAnswer(furtherPromptId) && IsPromptAnswerComplete(answers, furtherPromptId))
            {
                return new AdjustmentOutcome(new CompletedStudentAdjustment(studentId,
                    inclusionReasonId,
                    answers,
                    scrutinyReasonId,
                    rejectionReasonCode,
                    scrutinyStatusCode,
                    completionMessage,
                    OutcomeStatus.AutoAccept)
                    );
            }

            throw new Exception($"Prompt with id {furtherPromptId}, not found");
        }

        private string GetInfoPromptText(int promptId)
        {
            var prompt = _dataRepository.Get<Prompt>()
                .FirstOrDefault(p => p.PromptID == promptId);

            if (prompt != null)
            {
                return prompt.PromptText;
            }

            return "Prompt not found!";
        }

        #endregion
    }
}