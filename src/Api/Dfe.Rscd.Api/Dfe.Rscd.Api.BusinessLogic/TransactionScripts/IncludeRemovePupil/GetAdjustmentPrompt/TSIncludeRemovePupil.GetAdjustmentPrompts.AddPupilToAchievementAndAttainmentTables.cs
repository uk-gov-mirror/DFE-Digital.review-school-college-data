using System.Collections.Generic;
using System.Linq;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;


namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {
        private static AdjustmentPromptAnalysis GetAdjustmentPrompts_AddPupilToAchievementAndAttainmentTablesKS2(Web09_Entities context, Students student)
        {
            List<Prompts> promptListOut = new List<Prompts>();

            if (student == null ||
                student.PINCLs == null || student.PINCLs.P_INCL == null ||
                student.StudentChanges.First() == null || student.StudentChanges.First().YearGroups == null ||
                student.StudentChanges.First().YearGroups.YearGroupCode == null)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            
            if(student.StudentChanges.First().YearGroups.YearGroupCode.Trim() != "6" && TSStudent.HasKS2FutureResults(context, student.StudentID))
            {
                promptListOut.Add(GetPromptByPromptID(22110));
                promptListOut.Add(GetPromptByPromptID(Contants.PROMPT_ID_NC_YEAR_GROUP_KS2));
                return new AdjustmentPromptAnalysis(promptListOut);
            }
            else
            {
                return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(GetInfoPromptText(22120)));
            }
        }
    }
}
