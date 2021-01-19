using System.Collections.Generic;
using Dfe.Rscd.Api.BusinessLogic.Common;
using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services
{
    public partial class RemovePupilPromptsService
    {
        private static AdjustmentOutcome GetAdjustmentPrompts_AddPupilToAchievementAndAttainmentTablesKS2(Pupil student)
        {
            //List<Prompts> promptListOut = new List<Prompts>();

            //if (student == null ||
            //    student.PINCLs == null || student.PINCLs.P_INCL == null ||
            //    student.StudentChanges.First() == null || student.StudentChanges.First().YearGroups == null ||
            //    student.StudentChanges.First().YearGroups.YearGroupCode == null)
            //    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);


            //if (student.StudentChanges.First().YearGroups.YearGroupCode.Trim() != "6" && TSStudent.HasKS2FutureResults(context, student.StudentID))
            //{
            //    promptListOut.Add(GetPromptByPromptID(22110));
            //    promptListOut.Add(GetPromptByPromptID(Constants.PROMPT_ID_NC_YEAR_GROUP_KS2));
            //    return new AdjustmentOutcome(promptListOut);
            //}
            //else
            //{
            //    return new AdjustmentOutcome(new CompletedNonStudentAdjustment(GetInfoPromptText(22120)));
            //}

            return new AdjustmentOutcome(new CompletedNonStudentAdjustment("TODO"));
        }
    }
}
