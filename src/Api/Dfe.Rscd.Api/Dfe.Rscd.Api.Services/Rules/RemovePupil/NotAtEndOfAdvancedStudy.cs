using System;
using System.Collections.Generic;
using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services.Rules
{
    public partial class RemovePupilRulesV2
    {

        public static AdjustmentOutcome ProcessInclusionPromptResponses_NotAtEndOfAdvancedStudy(Pupil student, int inclusionReasonId, List<PromptAnswer> answers)
        {
            //if (student == null ||
            //    student.StudentChanges.First() == null ||
            //    student.StudentChanges.First().YearGroups == null ||
            //    student.StudentChanges.First().YearGroups.YearGroupCode == null)
            //    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            //int studentYearGroup;

            //if (int.TryParse(student.StudentChanges.First().YearGroups.YearGroupCode, out studentYearGroup) && studentYearGroup == 13)
            //{
            //    return ProcessKS5NCYearGroupAdjustment(student, inclusionReasonId, answers, 5420);
            //}
            //else
            //{
            //    return new AdjustmentOutcome(new CompletedNonStudentAdjustment(GetInfoPromptText(5410)));
            //}

            //throw new NotImplementedException();

            return new AdjustmentOutcome(new CompletedNonStudentAdjustment("TODO"));
        }
    }
}
