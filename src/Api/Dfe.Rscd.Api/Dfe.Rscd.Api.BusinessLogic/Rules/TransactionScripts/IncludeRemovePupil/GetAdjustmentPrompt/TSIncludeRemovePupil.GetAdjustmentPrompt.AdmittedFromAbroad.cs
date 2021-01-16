using System.Collections.Generic;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {

        

        private static List<Prompts> GetAdjustmentPrompts_AdmittedFromAbroad(Web09_Entities context, Students student, string pincl, int inclusionReasonId, int dfesNumber)
        {
            List<Prompts> promptsOut = new List<Prompts>();

            promptsOut = GetAllNonConditionalPromptsOnly(context, pincl, inclusionReasonId);

            if (student.Cohorts == null)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            if (TSSchool.IsSchoolNonPlasc(dfesNumber))
            {
                promptsOut.Add(GetPromptByPromptID(810));
            }
            else
            {
                if (student.Cohorts.KeyStage == 2 && pincl != Contants.PINCL_UNLISTED_PUPIL_WITH_ADD_PUPIL_REQUEST_PLASC_KS2)
                {
                    promptsOut.Add(GetPromptByPromptID(Contants.PROMPT_ID_REVISED_ADMISSION_DATE_IF_AVAILABLE));
                }
            }

            return promptsOut;

        }
        
    }
}
