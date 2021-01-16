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

        private static AdjustmentPromptAnalysis GetAdjustmentPrompts_AddPupilToAchievementAndAttainmentTablesKS5(Students student, int inclusionReasonId)
        {
            List<Prompts> promptListReturn = new List<Prompts>();

            if (student == null || student.StudentChanges.Count == 0)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            StudentChanges studentChange = student.StudentChanges.First();

            if (studentChange == null || studentChange.YearGroups == null)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            int yearGroup;
            if (int.TryParse(studentChange.YearGroups.YearGroupCode, out yearGroup) && yearGroup == 13)
            {
                //Return prompt 5710
                return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(student.StudentID,
                    inclusionReasonId,
                    new PromptAnswerList(),
                    Contants.SCRUTINY_REASON_ADD_PUPIL,
                    null,
                    Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                    GetInfoPromptText(5710))
                    );
            }
            else
            {
                //Return prompt 5720 and initialise prompt set for reason 52.
                return ProcessKS5NCYearGroupAdjustment(student, inclusionReasonId, new PromptAnswerList(), 5720);
            }
        }
    }
}
