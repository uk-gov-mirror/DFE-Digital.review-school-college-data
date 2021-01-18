using System.Collections.Generic;

namespace Dfe.Rscd.Api.BusinessLogic.Entities
{
    public static class PromptAnswerExtensions
    {
        public static PromptAnswer GetPromptAnswerByPromptId(this List<PromptAnswer> prompts, int promptId)
        {
            PromptAnswer rtnAnswer = null;

            foreach(PromptAnswer answer in prompts)
            {
                if (answer.PromptID == promptId)
                {
                    rtnAnswer = answer;
                    break;
                }
            }

            return rtnAnswer;
        }

        public static bool HasPromptAnswer(this List<PromptAnswer> prompts, int promptId)
        {
            bool returnHasPromptAnswer = false;
 
            foreach (PromptAnswer answer in prompts)
            {
                if (answer.PromptID == promptId)
                {
                    returnHasPromptAnswer = true;
                    break;
                }
            }

            return returnHasPromptAnswer;
        }

    }
}
