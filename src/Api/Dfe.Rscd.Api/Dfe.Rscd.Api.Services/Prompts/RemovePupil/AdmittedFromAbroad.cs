using System.Collections.Generic;
using Dfe.Rscd.Api.BusinessLogic.Common;
using Dfe.Rscd.Api.BusinessLogic.Entities;
using Prompt = Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs.Prompt;

namespace Dfe.Rscd.Api.Services
{
    public partial class RemovePupilPromptsService
    {
        private List<Prompt> AdmittedFromAbroad(string pinclCode, int inclusionReasonId)
        {
            var promptsOut = GetAllNonConditionalPromptsOnly(pinclCode, inclusionReasonId);

            if (_checkingWindow == CheckingWindow.KS2 &&
                pinclCode != Constants.PINCL_UNLISTED_PUPIL_WITH_ADD_PUPIL_REQUEST_PLASC_KS2)
            {
                promptsOut.Add(GetPromptByPromptId(Constants.PROMPT_ID_REVISED_ADMISSION_DATE_IF_AVAILABLE));
            }

            return promptsOut;
        }
    }
}
