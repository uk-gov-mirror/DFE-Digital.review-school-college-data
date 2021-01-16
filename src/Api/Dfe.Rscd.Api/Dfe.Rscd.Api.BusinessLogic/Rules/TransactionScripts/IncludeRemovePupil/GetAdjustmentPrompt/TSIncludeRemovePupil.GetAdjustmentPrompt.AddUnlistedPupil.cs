using System;
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
        private static AdjustmentPromptAnalysis GetAdjustmentPrompts_AddUnlistedPupilToAATKS2(Students student, int inclusionReasonId)
        {
            if (student.StudentChanges.Count == 0 ||
                student.StudentChanges.First() == null || student.StudentChanges.First().ENTRYDAT == null)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);  

            DateTime admissionDate = TSStudent.ConvertDateTimeDBString(student.StudentChanges.First().ENTRYDAT);

            //if (admissionDate <= KS2TestEndDate)
            //{
            //    return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
            //        student.StudentID,
            //        inclusionReasonId,
            //        null,
            //        Contants.SCRUTINY_REASON_ADD_UNLISTED_PUPIL,
            //        null,
            //        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
            //        GetInfoPromptText(9210))
            //        );
            //}
            //after the end date
            return new AdjustmentPromptAnalysis(new List<Prompts> { GetPromptByPromptID(9220) });
                      
        }

        private static AdjustmentPromptAnalysis GetAdjustmentPrompts_AddUnlistedPupilToAATKS4(Students student, int inclusionReasonId)
        {
            short yearGroup;

            if (student.StudentChanges.Count == 0 ||
                student.StudentChanges.First() == null || student.StudentChanges.First().ENTRYDAT == null ||
                student.StudentChanges.First().YearGroups == null ||
                !short.TryParse(student.StudentChanges.First().YearGroups.YearGroupCode, out yearGroup))
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            DateTime admissionDate = TSStudent.ConvertDateTimeDBString(student.StudentChanges.First().ENTRYDAT);
            
            if (yearGroup != 11)
            {
                return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(GetInfoPromptText(9430)));
            }
            if (admissionDate > AnnualSchoolCensusDate)
            {
                return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(GetInfoPromptText(9420)));
            }
            // => If this point is reached, Year group is 11 and admission date is <= the census date.
            return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                                                    student.StudentID,
                                                    inclusionReasonId,
                                                    null,
                                                    Contants.SCRUTINY_REASON_ADD_UNLISTED_PUPIL,
                                                    null,
                                                    Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                                    GetInfoPromptText(9410))
                );
        }

        private static AdjustmentPromptAnalysis GetAdjustmentPrompts_AddUnlistedStudentToAATKS5(Students student, int inclusionReasonId)
        {
            short yearGroup;
            
            if (student.StudentChanges.Count == 0 || student.StudentChanges.First() == null || 
                student.StudentChanges.First().DOB == null ||
                student.StudentChanges.First().YearGroups == null ||
                !short.TryParse(student.StudentChanges.First().YearGroups.YearGroupCode, out yearGroup))
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            DateTime dateOfBirth = TSStudent.ConvertDateTimeDBString(student.StudentChanges.First().DOB);
            DateTime? admissionDate = TSStudent.TryConvertDateTimeDBString(student.StudentChanges.First().ENTRYDAT);
            short studentAge = TSStudent.CalculateStudentAge(dateOfBirth);

            DateTime censusDate = AnnualSchoolCensusDate;

            if(admissionDate.HasValue && admissionDate > censusDate)
            {
                return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(GetInfoPromptText(9520)));
            }
            else if(yearGroup != 13)
            {
                return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(GetInfoPromptText(9530)));
            }
            else if (studentAge < 16 || studentAge > 18)
            {
                return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(GetInfoPromptText(9540)));
            }
            else 
            {
                //If the code reaches this point, the following if
                //statement is true: if(yearGroup == 13 && admissionDate <= censusDate && studentAge >= 16 && studentAge <= 18)
                return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                    student.StudentID,
                    inclusionReasonId,
                    null,
                    Contants.SCRUTINY_REASON_ADD_UNLISTED_PUPIL,
                    null,
                    Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                    GetInfoPromptText(9510))
                    );
            }
            
             

        }
    }
}
