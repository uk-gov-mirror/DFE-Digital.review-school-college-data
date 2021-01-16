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

        private static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_PupilNotAtEndOfKeyStage4(Students student, int inclusionReasonId, PromptAnswerList answers)
        {
            if(student == null || 
                student.StudentChanges.First() == null)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            return ProcessKS4NCYearGroupAdjustment(student,
                        inclusionReasonId,
                        answers,
                        1320);
        }

        private static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_PupilNotAtEndOfKeyStage2(Students student, int inclusionReasonId, PromptAnswerList answers)
        {
            if (student == null ||
                student.StudentChanges.First() == null ||
                student.StudentChanges.First().YearGroups == null ||
                student.StudentChanges.First().YearGroups.YearGroupCode == null ||
                student.PINCLs == null || student.PINCLs.P_INCL == null)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            List<Prompts> furtherPrompts = new List<Prompts>();
            int yearGroup;

            if (student.PINCLs.P_INCL == "202")
            {
                return ProcessExceptionalCircumstancesResponse(
                    student.StudentID, 
                    inclusionReasonId, 
                    answers, 
                    Contants.SCRUTINY_REASON_NOT_AT_END_OF_KEY_STAGE_IN_ALL_SUBJECTS, 
                    21310);
            }
            else if (int.TryParse(student.StudentChanges.First().YearGroups.YearGroupCode, out yearGroup))
            {
                if (yearGroup >= 6)
                {
                    
                    //Check that prompt 21320 has been displayed. If no, display it,
                    //but alter the string to include the assigned year group.
                    int promptForYearGroupGTSix = 21320;
                    if (!answers.HasPromptAnswer(promptForYearGroupGTSix))
                    {
                        Prompts prompt21320 = GetPromptByPromptID(promptForYearGroupGTSix);
                        prompt21320.PromptText = prompt21320.PromptText.Replace("[Year Group]", yearGroup.ToString());
                        return new AdjustmentPromptAnalysis(new List<Prompts> { prompt21320 });
                    }
                    else if (answers.HasPromptAnswer(promptForYearGroupGTSix) && IsPromptAnswerComplete(answers, promptForYearGroupGTSix))
                    {
                        return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(student.StudentID,
                            inclusionReasonId,
                            answers,
                            Contants.SCRUTINY_REASON_NOT_AT_END_OF_KEY_STAGE_IN_ALL_SUBJECTS,
                            null,
                            Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                            null)
                            );
                    }
                    else
                    {
                        throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
                    }

                }
                else //ie year group < 6
                {
                    return ProcessSingularFurtherPrompt(21330,
                        student.StudentID,
                        inclusionReasonId,
                        answers,
                        Contants.SCRUTINY_REASON_NOT_AT_END_OF_KEY_STAGE_IN_ALL_SUBJECTS,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        null);
                }
            }
            else
            {
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            }

        }
        
    }
}
