using System;
using System.Collections.Generic;
using Dfe.Rscd.Api.BusinessLogic.Common;
using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services
{
    public partial class RemovePupilPromptsService
    {
        private static AdjustmentOutcome AddUnlistedPupilToAATKS2(Pupil student, int inclusionReasonId)
        {
            //if (student.StudentChanges.Count == 0 ||
            //    student.StudentChanges.First() == null || student.StudentChanges.First().ENTRYDAT == null)
            //    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);  

            //DateTime admissionDate = TSStudent.ConvertDateTimeDBString(student.StudentChanges.First().ENTRYDAT);

            //return new AdjustmentOutcome(new List<Prompts> { GetPromptByPromptID(9220) });
            return new AdjustmentOutcome(new CompletedNonStudentAdjustment("TODO"));
        }

        private static AdjustmentOutcome AddUnlistedPupilToAATKS4(Pupil student, int inclusionReasonId)
        {
            //short yearGroup;

            //if (student.StudentChanges.Count == 0 ||
            //    student.StudentChanges.First() == null || student.StudentChanges.First().ENTRYDAT == null ||
            //    student.StudentChanges.First().YearGroups == null ||
            //    !short.TryParse(student.StudentChanges.First().YearGroups.YearGroupCode, out yearGroup))
            //    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            //DateTime admissionDate = TSStudent.ConvertDateTimeDBString(student.StudentChanges.First().ENTRYDAT);
            
            //if (yearGroup != 11)
            //{
            //    return new AdjustmentOutcome(new CompletedNonStudentAdjustment(GetInfoPromptText(9430)));
            //}
            //if (admissionDate > AnnualSchoolCensusDate)
            //{
            //    return new AdjustmentOutcome(new CompletedNonStudentAdjustment(GetInfoPromptText(9420)));
            //}
            //// => If this point is reached, Year group is 11 and admission date is <= the census date.
            //return new AdjustmentOutcome(new CompletedStudentAdjustment(
            //                                        student.StudentID,
            //                                        inclusionReasonId,
            //                                        null,
            //                                        Constants.SCRUTINY_REASON_ADD_UNLISTED_PUPIL,
            //                                        null,
            //                                        Constants.SCRUTINY_STATUS_PENDINGFORVUS,
            //                                        GetInfoPromptText(9410))
            //    );

            return new AdjustmentOutcome(new CompletedNonStudentAdjustment("TODO"));
        }

        private static AdjustmentOutcome AddUnlistedStudentToAATKS5(Pupil student, int inclusionReasonId)
        {
            //short yearGroup;
            
            //if (student.StudentChanges.Count == 0 || student.StudentChanges.First() == null || 
            //    student.StudentChanges.First().DOB == null ||
            //    student.StudentChanges.First().YearGroups == null ||
            //    !short.TryParse(student.StudentChanges.First().YearGroups.YearGroupCode, out yearGroup))
            //    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            //DateTime dateOfBirth = TSStudent.ConvertDateTimeDBString(student.StudentChanges.First().DOB);
            //DateTime? admissionDate = TSStudent.TryConvertDateTimeDBString(student.StudentChanges.First().ENTRYDAT);
            //short studentAge = TSStudent.CalculateStudentAge(dateOfBirth);

            //DateTime censusDate = AnnualSchoolCensusDate;

            //if(admissionDate.HasValue && admissionDate > censusDate)
            //{
            //    return new AdjustmentOutcome(new CompletedNonStudentAdjustment(GetInfoPromptText(9520)));
            //}
            //else if(yearGroup != 13)
            //{
            //    return new AdjustmentOutcome(new CompletedNonStudentAdjustment(GetInfoPromptText(9530)));
            //}
            //else if (studentAge < 16 || studentAge > 18)
            //{
            //    return new AdjustmentOutcome(new CompletedNonStudentAdjustment(GetInfoPromptText(9540)));
            //}
            //else 
            //{
            //    //If the code reaches this point, the following if
            //    //statement is true: if(yearGroup == 13 && admissionDate <= censusDate && studentAge >= 16 && studentAge <= 18)
            //    return new AdjustmentOutcome(new CompletedStudentAdjustment(
            //        student.StudentID,
            //        inclusionReasonId,
            //        null,
            //        Constants.SCRUTINY_REASON_ADD_UNLISTED_PUPIL,
            //        null,
            //        Constants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
            //        GetInfoPromptText(9510))
            //        );
            //}

            return new AdjustmentOutcome(new CompletedNonStudentAdjustment("TODO"));

        }
    }
}
