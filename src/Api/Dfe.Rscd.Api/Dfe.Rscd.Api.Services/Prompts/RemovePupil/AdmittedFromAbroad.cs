using System.Collections.Generic;
using Dfe.Rscd.Api.BusinessLogic.Common;
using Dfe.Rscd.Api.BusinessLogic.Entities;
using Prompt = Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs.Prompt;

namespace Dfe.Rscd.Api.Services
{
    public partial class RemovePupilPromptsService
    {
        private List<Prompt> GetAdjustmentPrompts_AdmittedFromAbroad(Pupil student, int inclusionReasonId, int dfesNumber)
        {
            var promptsOut = GetAllNonConditionalPromptsOnly(student.PINCL.P_INCL, inclusionReasonId);

            if (IsSchoolNonPlasc(dfesNumber.ToString()))
            {
                promptsOut.Add(GetPromptByPromptId(810));
            }
            else
            {
                if (_checkingWindow == CheckingWindow.KS2 && student.PINCL.P_INCL != Constants.PINCL_UNLISTED_PUPIL_WITH_ADD_PUPIL_REQUEST_PLASC_KS2)
                {
                    promptsOut.Add(GetPromptByPromptId(Constants.PROMPT_ID_REVISED_ADMISSION_DATE_IF_AVAILABLE));
                }
            }

            return promptsOut;
        }

        private bool IsSchoolNonPlasc(string dfesNumber)
        {
            var school = _establishmentService.GetByDFESNumber(_checkingWindow, dfesNumber);
            // What does school non plasc mean. TODO:
            return true;
        }
    }
}
