using System.Collections.Generic;
using System.Linq;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {

        private static List<Prompts> GetAdjustmentPrompts_PupilNotAtEndOfKeyStage4(Web09_Entities context, Students student)
        {
            List<Prompts> promptListReturn = new List<Prompts>();

            if (student == null || student.StudentChanges.Count == 0)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            StudentChanges studentChange = student.StudentChanges.First();

            if (studentChange == null || studentChange.YearGroups == null)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);
            

            int yearGroup;
            if (int.TryParse(studentChange.YearGroups.YearGroupCode, out yearGroup) && yearGroup == 11)
            {
                //Return prompt 1320, and prompt for NC Year Group.
                promptListReturn.Add(GetPromptByPromptID(1320));
                promptListReturn.Add(GetPromptByPromptID(Contants.PROMPT_ID_NC_YEAR_GROUP_KS4));
            }
            else
            {
                promptListReturn.Add(GetExceptionalCircumstancesPrompt(context));
            }

            return promptListReturn;
        }
    }
}
