using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.BusinessLogic.Entities;
using Dfe.Rscd.Api.Infrastructure.SqlServer.Repositories;

namespace Dfe.Rscd.Api.Services.Rules
{
    public abstract class BaseRules : IRuleSet
    {
        private readonly IDataRepository _dataRepository;

        protected BaseRules(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        protected IDataRepository DataRepository => _dataRepository;

        protected bool IsPromptAnswerComplete(List<PromptAnswer> promptAnswerList, int promptId)
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

        protected bool IsPromptAnswerComplete(PromptAnswer answer)
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

        protected AdjustmentOutcome ProcessSingularFurtherPrompt(int furtherPromptId,
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

        protected string GetInfoPromptText(int promptId)
        {
            var prompt = _dataRepository.Get<Prompt>()
                .FirstOrDefault(p => p.PromptID == promptId);

            if (prompt != null)
            {
                return prompt.PromptText;
            }

            return "Prompt not found!";
        }

        public abstract AmendmentType AmendmentType { get; }
        public abstract AdjustmentOutcome Apply(Amendment amendment);
    }
}
