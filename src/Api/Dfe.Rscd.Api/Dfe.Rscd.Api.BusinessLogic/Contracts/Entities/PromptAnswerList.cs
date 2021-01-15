using System.Collections.Generic;

namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Entities
{
    public class PromptAnswerList : List<PromptAnswer>
    {
        public PromptAnswer GetPromptAnswerByPromptID(int promptId)
        {
            PromptAnswer rtnAnswer = null;

            foreach(PromptAnswer answer in this)
            {
                if (answer.PromptID == promptId)
                {
                    rtnAnswer = answer;
                    break;
                }
            }

            return rtnAnswer;
        }

        public bool HasPromptAnswer(int promptId)
        {
            bool returnHasPromptAnswer = false;
 
            foreach (PromptAnswer answer in this)
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
