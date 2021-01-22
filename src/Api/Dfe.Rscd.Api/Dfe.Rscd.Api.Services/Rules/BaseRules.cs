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
            var prompt = _dataRepository.Get<Prompt>().Single(x => x.PromptID == answer.PromptID);

            bool promptAllowsNulls = prompt.AllowNulls;

            if (!string.IsNullOrEmpty(answer.PromptStringAnswer) || promptAllowsNulls)
                return true;

            return false;
        }

        protected AmendmentOutcome ProcessSingularFurtherPrompt(int furtherPromptId,
            string studentId,
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
                return new AmendmentOutcome(furtherPrompts);
            }

            if (answers.HasPromptAnswer(furtherPromptId) && IsPromptAnswerComplete(answers, furtherPromptId))
            {
                return new AmendmentOutcome(new CompletedStudentAdjustment(studentId,
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
        public abstract AmendmentOutcome Apply(Amendment amendment);
        public abstract CheckingWindow CheckingWindow { get; }
    }
}
