using System;
using System.Collections.Generic;
using System.Linq;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.Business.Logic.Validation;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {

        private static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_AdmittedFollowingPermanentExclusionFromMaintainedSchool(Students student, int inclusionReasonId, PromptAnswerList answers)
        {
            if (IsPromptAnswerComplete(answers, 1001) && IsPromptAnswerComplete(answers, 1002))
            {
                DateTime? currentAdmissionDate = new DateTime();
                if (student.StudentID != 0)
                {
                    string currentAdmissionDateStr = TSStudent.GetStudentAdmissionDate(student.StudentID);
                    currentAdmissionDate = TSStudent.TryConvertDateTimeDBString(currentAdmissionDateStr);
                }
                else
                {
                    currentAdmissionDate = TSStudent.TryConvertDateTimeDBString(student.StudentChanges.First().ENTRYDAT);
                }


                //Perform a check on the school.
                int schoolId = answers.GetPromptAnswerByPromptID(1001).PromptIntegerAnswer.Value;
                DateTime exclusionDate = answers.GetPromptAnswerByPromptID(1002).PromptDateTimeAnswer.Value;

                if(!TSSchool.IsSchoolRecognisedAndMaintained(schoolId))
                {
                    return ProcessSingularFurtherPrompt(1010,
                        student.StudentID,
                        inclusionReasonId,
                        answers,
                        Contants.SCRUTINY_REASON_ADMISSION_FOLLOWING_PERMANENT_EXCLUSION,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        null);
                }
                if (currentAdmissionDate.HasValue && exclusionDate > currentAdmissionDate.Value)
                {
                    return ProcessSingularFurtherPrompt(1020,
                                                        student.StudentID,
                                                        inclusionReasonId,
                                                        answers,
                                                        Contants.SCRUTINY_REASON_ADMISSION_FOLLOWING_PERMANENT_EXCLUSION,
                                                        null,
                                                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                                        null);
                }
                DateTime YearStartDateLessTwoYears = new DateTime((CurrentYear - 2), 9, 1);

                if(exclusionDate < YearStartDateLessTwoYears)
                {
                    return ProcessSingularFurtherPrompt(1030,
                                                        student.StudentID,
                                                        inclusionReasonId,
                                                        answers,
                                                        Contants.SCRUTINY_REASON_ADMISSION_FOLLOWING_PERMANENT_EXCLUSION,
                                                        null,
                                                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                                        null);
                }
                if (!TSStudent.IsStudentListed(student.StudentID))
                {
                    var pa = new CompletedStudentAdjustment(
                        student.StudentID,
                        inclusionReasonId,
                        answers,
                        Contants.SCRUTINY_REASON_ADMISSION_FOLLOWING_PERMANENT_EXCLUSION,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        GetInfoPromptText(1040));
                    var startDateTest = new DateTime((CurrentYear - 1), 9, 1);
                    if(exclusionDate < startDateTest)
                    {
                        pa.ErrorMessageList = new List<Student.ValidationFailure>
                                                  {
                                                      new Student.ValidationFailure()
                                                          {
                                                              Message =
                                                                  "Date is before 1 September " +
                                                                  startDateTest.Year.ToString()
                                                          }
                                                  };
                    }
                    return new AdjustmentPromptAnalysis(pa);
                }
                return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                                                        student.StudentID,
                                                        inclusionReasonId,
                                                        answers,
                                                        Contants.SCRUTINY_REASON_ADMISSION_FOLLOWING_PERMANENT_EXCLUSION,
                                                        null,
                                                        Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                                                        null)
                    );
            }
            //Insufficient answers provided.
            throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
        }
    }
}
