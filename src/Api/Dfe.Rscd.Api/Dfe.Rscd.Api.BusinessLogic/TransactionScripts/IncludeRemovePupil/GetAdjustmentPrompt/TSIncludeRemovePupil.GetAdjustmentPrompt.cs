using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil : Logic.TSBase
    {

        public static AdjustmentPromptAnalysis GetAdjustmentPrompts(int dfesNumber, Students student, int inclusionReasonId, StudentAdjustmentType adjustmentType)
        {

            List<Prompts> promptsListOut = new List<Prompts>();

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {

                    string studentPincl = student.PINCLs.P_INCL;



                    return GetAdjustmentPrompts(context, dfesNumber, student, inclusionReasonId, studentPincl);

                }
            }
        }


        public static AdjustmentPromptAnalysis GetAdjustmentPrompts(int dfesNumber, Students student, int inclusionReasonId, string pincl)
        {

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return GetAdjustmentPrompts(context, dfesNumber, student, inclusionReasonId, pincl);
                }
            }
        }




        #region Private methods

        private static AdjustmentPromptAnalysis GetAdjustmentPrompts(Web09_Entities context, int dfesNumber, Students student, int inclusionReasonId, string pincl)
        {
            
            List<Prompts> promptsListOut = new List<Prompts>();

            switch (inclusionReasonId)
            {

                //Reason 3
                case ((int)ReasonsForAdjustment.CancelAddBack):
                    promptsListOut = GetAllPrompts(context, pincl, inclusionReasonId);
                    break;
                //Reason 4
                case ((int)ReasonsForAdjustment.CancelAdjustmentForAdmissionFollowingPermanentExclusion):
                    promptsListOut = GetAllPrompts(context, pincl, inclusionReasonId);
                    break;
                //Reason 5
                case ((int)ReasonsForAdjustment.RemovePupilCompletelyFromAllData):
                    promptsListOut = GetAllPrompts(context, pincl, inclusionReasonId);
                    break;
                //Reason 6
                case ((int)ReasonsForAdjustment.PublishPupilAtThisSchool):
                    promptsListOut = GetAllPrompts(context, pincl, inclusionReasonId);
                    break;
                //Reason 7
                case ((int)ReasonsForAdjustment.ReinstateThePupilKS4):
                    promptsListOut = GetAllPrompts(context, pincl, inclusionReasonId);
                    break;

                //Reason 8
                case ((int)ReasonsForAdjustment.AdmittedFromAbroad):
                    promptsListOut = GetAdjustmentPrompts_AdmittedFromAbroad(context, student, pincl, inclusionReasonId, dfesNumber);
                    break;

                //Reason 9
                case ((int)ReasonsForAdjustment.ContingencyKS4):
                    promptsListOut = GetAllPrompts(context, pincl, inclusionReasonId);
                    break;

                //Reason 10
                case ((int)ReasonsForAdjustment.AdmittedFollowingPermanentExclusionFromMaintainedSchool):
                    promptsListOut = GetAllNonConditionalPromptsOnly(context, pincl, inclusionReasonId);
                    break;

                //Reason 11
                case ((int)ReasonsForAdjustment.PermanentlyLeftEngland):
                    promptsListOut = GetAllNonConditionalPromptsOnly(context, pincl, inclusionReasonId);
                    break;
                
                //Reason 12
                case ((int)ReasonsForAdjustment.Deceased):
                    promptsListOut = GetAllNonConditionalPromptsOnly(context, pincl, inclusionReasonId);
                    break;

                //Reason 13
                case ((int)ReasonsForAdjustment.PupilNotAtEndOfKeyStage4):
                    promptsListOut = GetAdjustmentPrompts_PupilNotAtEndOfKeyStage4(context, student);
                    break;

                //Reason 17
                case ((int)ReasonsForAdjustment.PupilAtEndOfKeyStage4):
                    promptsListOut.Add(GetPromptByPromptID(1700));
                    promptsListOut.Add(GetPromptByPromptID((int)Contants.PROMPT_ID_NC_YEAR_GROUP_KS4));
                    break;

                //Reason 18 
                case ((int)ReasonsForAdjustment.LeftSchoolRollBeforeExamsKS4):
                    promptsListOut = GetAllNonConditionalPromptsOnly(context, pincl, inclusionReasonId);
                    break;

                //Reason 19 
                case ((int)ReasonsForAdjustment.KS4Other):
                    promptsListOut = GetAllNonConditionalPromptsOnly(context, pincl, inclusionReasonId);
                    break;
                
                //Reason 21 
                case ((int)ReasonsForAdjustment.AddPupilToAchievementAndAttainmentTablesKS4):
                    //Retrieve all prompts for reason 221 along with prompts for reason 22 and 14.
                    promptsListOut.Add(GetPromptByPromptID(2100));
                    promptsListOut.Add(GetPromptByPromptID((int)Contants.PROMPT_ID_ADMISSION_DATE_KS4));
                    promptsListOut.Add(GetPromptByPromptID((int)Contants.PROMPT_ID_NC_YEAR_GROUP_KS4));
                    break;
                
                //Reason 23 
                case ((int)ReasonsForAdjustment.ResultsBelongToAnotherPupilKS4):
                    promptsListOut = GetAllPrompts(context, pincl, inclusionReasonId);
                    break;
                
                //Reason 54 
                case ((int)ReasonsForAdjustment.NotAtEndOfAdvancedStudy ):
                    return GetAdjustmentPrompts_NotAtEndOfAdvancedStudy(student, inclusionReasonId);
                    
                
                //Reason 55 
                case ((int)ReasonsForAdjustment.LeftBeforeExamsKS5):
                    promptsListOut = GetAllPrompts(context, pincl, inclusionReasonId);
                    break;
                
                //Reason 56 
                case ((int)ReasonsForAdjustment.KS5Other):
                    promptsListOut = GetAllNonConditionalPromptsOnly(context, pincl, inclusionReasonId);
                    break;
                
                //Reason 57 
                case ((int)ReasonsForAdjustment.AddPupilToAchievementAndAttainmentTablesKS5 ):
                    return GetAdjustmentPrompts_AddPupilToAchievementAndAttainmentTablesKS5(student, inclusionReasonId);
                
                //Reason 58 
                case ((int)ReasonsForAdjustment.ResultsBelongToAnotherPupilKS5):
                    promptsListOut = GetAllPrompts(context, pincl, inclusionReasonId);
                    break;
                
                //Reason 59 
                case ((int)ReasonsForAdjustment.ReinstatePupilKS5):
                    promptsListOut = GetAllPrompts(context, pincl, inclusionReasonId);
                    break;

                //Reason 92
                case ((int)ReasonsForAdjustment.AddUnlistedPupilToAATKS2):
                    return GetAdjustmentPrompts_AddUnlistedPupilToAATKS2(student, inclusionReasonId);

                //Reason 94
                case((int)ReasonsForAdjustment.AddUnlistedPupilToAATKS4):
                    return GetAdjustmentPrompts_AddUnlistedPupilToAATKS4(student, inclusionReasonId);

                //Reason 95
                case((int)ReasonsForAdjustment.AddUnlistedStudentToAATKS5):
                    return GetAdjustmentPrompts_AddUnlistedStudentToAATKS5(student, inclusionReasonId);

                //Reason 212
                case ((int)ReasonsForAdjustment.ContingencyKS2):
                    promptsListOut = GetAllPrompts(context, pincl, inclusionReasonId);
                    break;

                //Reason 213
                case ((int)ReasonsForAdjustment.PupilNotAtEndOfKeyStage2InAllSubjects):
                    promptsListOut = GetAdjustmentPrompts_PupilNotAtEndOfKeyStage2InAllSubjects(context, student);
                    break;

                //Reason 218
                case ((int)ReasonsForAdjustment.LeftSchoolRollBeforeTestsKS2):
                    promptsListOut = GetAllNonConditionalPromptsOnly(context, pincl, inclusionReasonId);
                    break;

                //Reason 219
                case ((int)ReasonsForAdjustment.KS2Other):
                    promptsListOut = GetAllPrompts(context, pincl, inclusionReasonId);
                    break;

                //Reason 221
                case ((int)ReasonsForAdjustment.AddPupilToAchievementAndAttainmentTablesKS2):
                    //Retrieve all prompts for reason 221 along with prompts for 214.
                    return GetAdjustmentPrompts_AddPupilToAchievementAndAttainmentTablesKS2(context, student);

                //Reason 223
                case ((int)ReasonsForAdjustment.ResultsBelongToAnotherPupilKS2):
                    promptsListOut = GetAllPrompts(context, pincl, inclusionReasonId);
                    break;

                //Reason 313
                case ((int)ReasonsForAdjustment.PupilNotAtEndOfKeyStage3InAllSubjects):
                    student.StudentChanges.Add(student.StudentChanges.First());
                    promptsListOut = GetAdjustmentPrompts_PupilNotAtEndOfKeyStage3InAllSubjects(context, student);
                    break;

                //Reason 318
                case ((int)ReasonsForAdjustment.LeftSchoolRollBeforeTestsKS3):
                    promptsListOut = GetAllNonConditionalPromptsOnly(context, pincl, inclusionReasonId);
                    break;

                //Reason 319
                case ((int)ReasonsForAdjustment.KS3Other):
                    promptsListOut = GetAllNonConditionalPromptsOnly(context, pincl, inclusionReasonId);
                    break;
                
                //Reason 321 
                case ((int)ReasonsForAdjustment.AddPupilToAchievementAndAttainmentTablesKS3 ):
                    //Retrieve all prompts for reason 221 along with prompts for 314.
                    promptsListOut = GetAllPrompts(context, pincl, inclusionReasonId);
                    AdjustmentPromptAnalysis ncYearGroupAdjPrompts = GetAdjustmentPrompts(dfesNumber, student, 314, pincl);
                    promptsListOut.AddRange(ncYearGroupAdjPrompts.FurtherPrompts);
                    break;

                //Reason 323
                case ((int)ReasonsForAdjustment.ResultsBelongToAnotherPupilKS3):
                    promptsListOut = GetAllPrompts(context, pincl, inclusionReasonId);
                    break;

                //Reason 324
                case ((int)ReasonsForAdjustment.CancelAddBackKS5):
                    promptsListOut = GetAllPrompts(context, pincl, inclusionReasonId);
                    break;

                default:
                    break;

            }

            return new AdjustmentPromptAnalysis(promptsListOut);
        }

        
        private static List<Prompts> GetAllNonConditionalPromptsOnly(Web09_Entities context, string pincl, int inclusionReasonId)
        {
            IQueryable<PINCLInclusionAdjustments> query = GetAllPINCLInclusionAdjustments(context, pincl, inclusionReasonId);

            var inclusionReason = query.Select(ir => ir).ToList();

            PINCLInclusionAdjustments pinclInclusionAdjustments = inclusionReason[0];
            return pinclInclusionAdjustments.Prompts.Where(p => p.IsConditional == false).Select(p => p).ToList();
        }

        private static List<Prompts> GetAllPrompts(Web09_Entities context, string pincl, int inclusionReasonId)
        {
            //Retrieve the relevant prompts.
            IQueryable<PINCLInclusionAdjustments> query = GetAllPINCLInclusionAdjustments(context, pincl, inclusionReasonId);

            var inclusionReason = query.Select(ir => ir).ToList();

            if (inclusionReason.Any())
            {
                PINCLInclusionAdjustments pinclInclusionAdjustments = inclusionReason[0];
                return pinclInclusionAdjustments.Prompts.Select(p => p).ToList();
            }

            return new List<Prompts>();
        }

        private static IQueryable<PINCLInclusionAdjustments> GetAllPINCLInclusionAdjustments(Web09_Entities context, string pincl, int inclusionReasonId)
        {
            return context.PINCLInclusionAdjustments
                .Include("InclusionAdjustmentReasons")
                .Include("Prompts")
                .Include("Prompts.PromptTypes")
                .Include("Prompts.PromptResponses")
                .Where(pia => pia.P_INCL == pincl && pia.InclusionAdjustmentReasons.IncAdjReasonID == inclusionReasonId)
                .Select(pia => pia);
        }

        private static Prompts GetPromptByPromptID(Web09_Entities context, int promptId)
        {
            return context.Prompts
                .Include("PromptTypes")
                .Include("PromptResponses")
                .Where(p => p.PromptID == promptId)
                .First();
        }

        private static string GetListItemPromptDisplayText(Web09_Entities context, PromptAnswer answer)
        {
            short promptId = short.Parse(answer.PromptID.ToString());
            short selectedValue = short.Parse(answer.PromptSelectedValueAnswer);

            return context.PromptResponses
                .Where(pr => pr.PromptID == promptId && pr.ListOrder == selectedValue)
                .Select(pr => pr.ListValue)
                .First();
        }

        /// <summary>
        /// Retrieve a singular prompt with a given prompt id. Use this overloaded method
        /// where no context object is available from calling object.
        /// </summary>
        /// <param name="promptID">PromptID to collect prompt item from Scrutiny.Prompts</param>
        /// <returns>Prompts Item from given promptID</returns>
        internal static Prompts GetPromptByPromptID(int promptID)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return GetPromptByPromptID(context, promptID);
                }
            }
        }

        private static string GetListItemPromptDisplayText(PromptAnswer answer)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return GetListItemPromptDisplayText(context, answer);
                }
            }
        }

        /// <summary>
        /// Retrieve prompt 20. This is cross referenced by multiple reasons.
        /// </summary>
        /// <param name="context">The Web09_Entities context object to use to 
        /// retrieve the Exceptional Circumstances prompt</param>
        /// <returns>The Exceptional Circumstances prompt</returns>
        private static Prompts GetExceptionalCircumstancesPrompt(Web09_Entities context)
        {
            return context.Prompts
                .Include("PromptTypes")
                .FirstOrDefault(p => p.PromptID == (int)Contants.PROMPT_ID_EXCEPTIONALCIRCUMSTANCES);
        }

        #endregion

    }
}
