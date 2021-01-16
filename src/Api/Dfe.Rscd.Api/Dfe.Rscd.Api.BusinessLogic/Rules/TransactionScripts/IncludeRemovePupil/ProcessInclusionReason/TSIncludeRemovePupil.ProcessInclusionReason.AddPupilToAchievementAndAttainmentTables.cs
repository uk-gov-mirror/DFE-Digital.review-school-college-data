using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {

        private static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_AddPupilToAchievementAndAttainmentTablesKS2(Students student, int inclusionReasonId, PromptAnswerList answers)
        {

            if (student == null ||
                student.PINCLs == null || student.PINCLs.P_INCL == null ||
                student.StudentChanges.First() == null || student.StudentChanges.First().YearGroups == null ||
                student.StudentChanges.First().YearGroups.YearGroupCode == null)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    if (student.StudentChanges.First().YearGroups.YearGroupCode.Trim() != "6" && TSStudent.HasKS2FutureResults(context, student.StudentID))
                    {
                        return ProcessKS2NCYearGroupAdjustment(context, student, inclusionReasonId, answers, 22110);
                    }
                    else
                    {
                        return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(GetInfoPromptText(22120)));
                    }
                }
            }
        }

        private static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_AddPupilToAchievementAndAttainmentTablesKS4(Students student, int inclusionReasonId, PromptAnswerList answers)
        {
            //This is a multiple request situation.
            bool isRequestComplete = true;
            AdjustmentPromptAnalysis NCYearGroupAnalysis = ProcessKS4NCYearGroupAdjustment(student, inclusionReasonId, answers, 2100);
            AdjustmentPromptAnalysis DateOnRoleAnalysis = ProcessAdmissionDateForKS4(student.StudentID, inclusionReasonId, answers);

            List<Prompts> furtherPrompts = new List<Prompts>();

            if (!NCYearGroupAnalysis.IsComplete )
            {
                furtherPrompts.AddRange(NCYearGroupAnalysis.FurtherPrompts);
                isRequestComplete = false;
            }
            

            if (!DateOnRoleAnalysis.IsComplete)
            {
                furtherPrompts.AddRange(DateOnRoleAnalysis.FurtherPrompts);
                isRequestComplete = false;
            }

            if (!isRequestComplete)
            {
                return new AdjustmentPromptAnalysis(furtherPrompts);
            }
            else
            {
                if(NCYearGroupAnalysis.IsAdjustmentCreated && DateOnRoleAnalysis.IsAdjustmentCreated)
                {
                    return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(student.StudentID,
                        inclusionReasonId,
                        answers,
                        Contants.SCRUTINY_REASON_MULTIPLE_REQUESTS,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        null)
                        );
                }
                else
                {
                    //If only one of the requests did not generate a request,
                    //continue with the other request. Otherwise, if both request
                    //reasons produced non-requests, return a non request.
                    if(NCYearGroupAnalysis.IsAdjustmentCreated && !DateOnRoleAnalysis.IsAdjustmentCreated)
                        return NCYearGroupAnalysis;
                    else if(!NCYearGroupAnalysis.IsAdjustmentCreated && DateOnRoleAnalysis.IsAdjustmentCreated)
                        return DateOnRoleAnalysis;
                    else
                        return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment("No request was generated."));
                            
                }
            }
            

        }

        private static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_AddPupilToAchievementAndAttainmentTablesKS5(Students student, int inclusionReasonId, PromptAnswerList answers)
        {
            if (student == null ||
                student.StudentChanges.First() == null ||
                student.StudentChanges.First().YearGroups == null || student.StudentChanges.First().YearGroups.YearGroupCode == null)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);


            if (student.StudentChanges.First().YearGroups.YearGroupCode != "13")
            {
                return ProcessKS5NCYearGroupAdjustment(student, inclusionReasonId, answers, 5720);
            }
            else //=> year group is 13.
            {
                return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(student.StudentID,
                    inclusionReasonId,
                    answers,
                    Contants.SCRUTINY_REASON_ADD_PUPIL,
                    null,
                    Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                    GetInfoPromptText(5710))
                    );
            }


        }
    }
}
