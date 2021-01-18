using System.Collections.Generic;
using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services
{
    public partial class RemovePupilPromptsService
    {
        public static AdjustmentOutcome GetAdjustmentPrompts_NotAtEndOfAdvancedStudy(Pupil student, int inclusionReasonId)
        {
            //List<Prompts> promptListReturn = new List<Prompts>();
            
            //if (student == null || student.StudentChanges.Count == 0)
            //    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            //StudentChanges studentChange = student.StudentChanges.First();

            //if(studentChange == null || studentChange.YearGroups == null)
            //    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            //int yearGroup;
            //if (int.TryParse(studentChange.YearGroups.YearGroupCode, out yearGroup) && yearGroup == 13)
            //{
            //    //Return prompt 5420, and prompt for NC Year Group.
            //    return ProcessKS5NCYearGroupAdjustment(student, inclusionReasonId, new PromptAnswerList(), 5420);
            //}
            //else
            //{
            //    return new AdjustmentOutcome(new CompletedNonStudentAdjustment(GetInfoPromptText(5420)));
            //}

            return new AdjustmentOutcome(new CompletedNonStudentAdjustment("TODO"));

        }
    }
}
