using System;
using System.Linq;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {

        public static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_NotAtEndOfAdvancedStudy(Students student, int inclusionReasonId, PromptAnswerList answers)
        {
            if (student == null ||
                student.StudentChanges.First() == null ||
                student.StudentChanges.First().YearGroups == null ||
                student.StudentChanges.First().YearGroups.YearGroupCode == null)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            int studentYearGroup;

            if (int.TryParse(student.StudentChanges.First().YearGroups.YearGroupCode, out studentYearGroup) && studentYearGroup == 13)
            {
                return ProcessKS5NCYearGroupAdjustment(student, inclusionReasonId, answers, 5420);
            }
            else
            {
                return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(GetInfoPromptText(5410)));
            }

            throw new NotImplementedException();
        }
    }
}
