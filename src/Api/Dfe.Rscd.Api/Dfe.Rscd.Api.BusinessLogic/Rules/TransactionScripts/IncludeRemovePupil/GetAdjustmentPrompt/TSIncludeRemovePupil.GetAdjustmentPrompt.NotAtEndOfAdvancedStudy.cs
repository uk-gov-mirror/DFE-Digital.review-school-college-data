using System.Collections.Generic;
using System.Linq;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {
        public static AdjustmentPromptAnalysis GetAdjustmentPrompts_NotAtEndOfAdvancedStudy(Students student, int inclusionReasonId)
        {
            List<Prompts> promptListReturn = new List<Prompts>();
            
            if (student == null || student.StudentChanges.Count == 0)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            StudentChanges studentChange = student.StudentChanges.First();

            if(studentChange == null || studentChange.YearGroups == null)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            int yearGroup;
            if (int.TryParse(studentChange.YearGroups.YearGroupCode, out yearGroup) && yearGroup == 13)
            {
                //Return prompt 5420, and prompt for NC Year Group.
                return ProcessKS5NCYearGroupAdjustment(student, inclusionReasonId, new PromptAnswerList(), 5420);
            }
            else
            {
                return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(GetInfoPromptText(5420)));
            }
            
        }
    }
}
