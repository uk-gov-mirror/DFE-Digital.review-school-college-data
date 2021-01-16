using System;
using System.Linq;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {

        private static AdjustmentPromptAnalysis AddUnlistedPupilToAATKS2(Students student, int inclusionReasonId, PromptAnswerList answers)
        {
            if (student.StudentChanges.Count == 0 ||
                student.StudentChanges.First() == null || student.StudentChanges.First().ENTRYDAT == null)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            DateTime admissionDate = TSStudent.ConvertDateTimeDBString(student.StudentChanges.First().ENTRYDAT);

            if (admissionDate <= KS2TestEndDate)
            {
                return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                    student.StudentID,
                    inclusionReasonId,
                    null,
                    Contants.SCRUTINY_REASON_ADD_UNLISTED_PUPIL,
                    null,
                    Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                    GetInfoPromptText(9210))
                    );
            }            
            // => admission date is after KS2 test end date
            return ProcessSingularFurtherPrompt(9200,
                student.StudentID,
                inclusionReasonId,
                answers,
                Contants.SCRUTINY_REASON_ADD_UNLISTED_PUPIL,
                null,
                Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                GetInfoPromptText(9210));
        }

    }

}