using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.BusinessLogic.Common;
using Dfe.Rscd.Api.BusinessLogic.Entities;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;
using Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs;
using Dfe.Rscd.Api.Infrastructure.SqlServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Prompt = Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs.Prompt;
using PromptType = Dfe.Rscd.Api.BusinessLogic.Entities.PromptType;

namespace Dfe.Rscd.Api.Services
{
    public partial class RemovePupilPromptsService : IPromptService
    {
        private readonly IDataRepository _repository;
        private readonly IPupilService _pupilService;
        private readonly IEstablishmentService _establishmentService;
        private readonly IAllocationYearConfig _allocationYear;
        private CheckingWindow _checkingWindow;

        public AmendmentType AmendmentType => AmendmentType.RemovePupil;

        public RemovePupilPromptsService(IDataRepository repository, IPupilService pupilService, IEstablishmentService establishmentService, IAllocationYearConfig allocationYear)
        {
            _repository = repository;
            _pupilService = pupilService;
            _establishmentService = establishmentService;
            _allocationYear = allocationYear;
        }

        public AdjustmentOutcome GetAdjustmentPrompts(CheckingWindow checkingWindow, int dfesNumber, string studendId, int inclusionReasonId)
        {
            _checkingWindow = checkingWindow;

            List<Prompts> promptsListOut = new List<Prompts>();
            var pupil = _pupilService.GetById(checkingWindow, studendId);

            return GetAdjustmentPrompts(dfesNumber, pupil, inclusionReasonId);
        }

        #region Private methods

        internal bool IsStudentYearGroupLessThanValue(string yearGroupCode, int compareValue)
        {
            int yearGroup;
            if (int.TryParse(yearGroupCode, out yearGroup))
            {
                return (yearGroup < compareValue) ? true : false;
            }
            else
            {
                return false;
            }
        }

        #endregion

        internal string GetScrutinyStatusDescriptionString(string scrutinyStatusCode)
        {
            // TODO: Hook up description
            return scrutinyStatusCode;
        }

        private AdjustmentOutcome GetAdjustmentPrompts(int dfesNumber, Pupil student, int inclusionReasonId)
        {
            
            List<Prompt> promptsListOut = new List<Prompt>();

            switch (inclusionReasonId)
            {

                //Reason 3
                case ((int)ReasonsForAdjustment.CancelAddBack):
                    promptsListOut = GetAllPrompts(student.PINCL.P_INCL, inclusionReasonId);
                    break;
                //Reason 4
                case ((int)ReasonsForAdjustment.CancelAdjustmentForAdmissionFollowingPermanentExclusion):
                    promptsListOut = GetAllPrompts(student.PINCL.P_INCL, inclusionReasonId);
                    break;
                //Reason 5
                case ((int)ReasonsForAdjustment.RemovePupilCompletelyFromAllData):
                    promptsListOut = GetAllPrompts(student.PINCL.P_INCL, inclusionReasonId);
                    break;
                //Reason 6
                case ((int)ReasonsForAdjustment.PublishPupilAtThisSchool):
                    promptsListOut = GetAllPrompts(student.PINCL.P_INCL, inclusionReasonId);
                    break;
                //Reason 7
                case ((int)ReasonsForAdjustment.ReinstateThePupilKS4):
                    promptsListOut = GetAllPrompts(student.PINCL.P_INCL, inclusionReasonId);
                    break;

                //Reason 8
                case ((int)ReasonsForAdjustment.AdmittedFromAbroad):
                    promptsListOut = GetAdjustmentPrompts_AdmittedFromAbroad(student, inclusionReasonId, dfesNumber);
                    break;

                //Reason 9
                case ((int)ReasonsForAdjustment.ContingencyKS4):
                    promptsListOut = GetAllPrompts(student.PINCL.P_INCL, inclusionReasonId);
                    break;

                //Reason 10
                case ((int)ReasonsForAdjustment.AdmittedFollowingPermanentExclusionFromMaintainedSchool):
                    promptsListOut = GetAllNonConditionalPromptsOnly(student.PINCL.P_INCL, inclusionReasonId);
                    break;

                //Reason 11
                case ((int)ReasonsForAdjustment.PermanentlyLeftEngland):
                    promptsListOut = GetAllNonConditionalPromptsOnly(student.PINCL.P_INCL, inclusionReasonId);
                    break;
                
                //Reason 12
                case ((int)ReasonsForAdjustment.Deceased):
                    promptsListOut = GetAllNonConditionalPromptsOnly(student.PINCL.P_INCL, inclusionReasonId);
                    break;

                //Reason 13
                case ((int)ReasonsForAdjustment.PupilNotAtEndOfKeyStage4):
                    promptsListOut = GetAdjustmentPrompts_PupilNotAtEndOfKeyStage4(student);
                    break;

                //Reason 17
                case ((int)ReasonsForAdjustment.PupilAtEndOfKeyStage4):
                    promptsListOut.Add(GetPromptByPromptId(1700));
                    promptsListOut.Add(GetPromptByPromptId((int)Constants.PROMPT_ID_NC_YEAR_GROUP_KS4));
                    break;

                //Reason 18 
                case ((int)ReasonsForAdjustment.LeftSchoolRollBeforeExamsKS4):
                    promptsListOut = GetAllNonConditionalPromptsOnly(student.PINCL.P_INCL, inclusionReasonId);
                    break;

                //Reason 19 
                case ((int)ReasonsForAdjustment.KS4Other):
                    promptsListOut = GetAllNonConditionalPromptsOnly(student.PINCL.P_INCL, inclusionReasonId);
                    break;
                
                //Reason 21 
                case ((int)ReasonsForAdjustment.AddPupilToAchievementAndAttainmentTablesKS4):
                    //Retrieve all prompts for reason 221 along with prompts for reason 22 and 14.
                    promptsListOut.Add(GetPromptByPromptId(2100));
                    promptsListOut.Add(GetPromptByPromptId((int)Constants.PROMPT_ID_ADMISSION_DATE_KS4));
                    promptsListOut.Add(GetPromptByPromptId((int)Constants.PROMPT_ID_NC_YEAR_GROUP_KS4));
                    break;
                
                //Reason 23 
                case ((int)ReasonsForAdjustment.ResultsBelongToAnotherPupilKS4):
                    promptsListOut = GetAllPrompts(student.PINCL.P_INCL, inclusionReasonId);
                    break;
                
                //Reason 54 
                case ((int)ReasonsForAdjustment.NotAtEndOfAdvancedStudy ):
                    return GetAdjustmentPrompts_NotAtEndOfAdvancedStudy(student, inclusionReasonId);
                    
                
                //Reason 55 
                case ((int)ReasonsForAdjustment.LeftBeforeExamsKS5):
                    promptsListOut = GetAllPrompts(student.PINCL.P_INCL, inclusionReasonId);
                    break;
                
                //Reason 56 
                case ((int)ReasonsForAdjustment.KS5Other):
                    promptsListOut = GetAllNonConditionalPromptsOnly(student.PINCL.P_INCL, inclusionReasonId);
                    break;
                
                //Reason 57 
                case ((int)ReasonsForAdjustment.AddPupilToAchievementAndAttainmentTablesKS5 ):
                    return GetAdjustmentPrompts_AddPupilToAchievementAndAttainmentTablesKS5(student, inclusionReasonId);
                
                //Reason 58 
                case ((int)ReasonsForAdjustment.ResultsBelongToAnotherPupilKS5):
                    promptsListOut = GetAllPrompts(student.PINCL.P_INCL, inclusionReasonId);
                    break;
                
                //Reason 59 
                case ((int)ReasonsForAdjustment.ReinstatePupilKS5):
                    promptsListOut = GetAllPrompts(student.PINCL.P_INCL, inclusionReasonId);
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
                    promptsListOut = GetAllPrompts(student.PINCL.P_INCL, inclusionReasonId);
                    break;

                //Reason 213
                case ((int)ReasonsForAdjustment.PupilNotAtEndOfKeyStage2InAllSubjects):
                    promptsListOut = GetAdjustmentPrompts_PupilNotAtEndOfKeyStage2InAllSubjects(student);
                    break;

                //Reason 218
                case ((int)ReasonsForAdjustment.LeftSchoolRollBeforeTestsKS2):
                    promptsListOut = GetAllNonConditionalPromptsOnly(student.PINCL.P_INCL, inclusionReasonId);
                    break;

                //Reason 219
                case ((int)ReasonsForAdjustment.KS2Other):
                    promptsListOut = GetAllPrompts(student.PINCL.P_INCL, inclusionReasonId);
                    break;

                //Reason 221
                case ((int)ReasonsForAdjustment.AddPupilToAchievementAndAttainmentTablesKS2):
                    //Retrieve all prompts for reason 221 along with prompts for 214.
                    return GetAdjustmentPrompts_AddPupilToAchievementAndAttainmentTablesKS2(student);

                //Reason 223
                case ((int)ReasonsForAdjustment.ResultsBelongToAnotherPupilKS2):
                    promptsListOut = GetAllPrompts(student.PINCL.P_INCL, inclusionReasonId);
                    break;

                //Reason 318
                case ((int)ReasonsForAdjustment.LeftSchoolRollBeforeTestsKS3):
                    promptsListOut = GetAllNonConditionalPromptsOnly(student.PINCL.P_INCL, inclusionReasonId);
                    break;

                //Reason 319
                case ((int)ReasonsForAdjustment.KS3Other):
                    promptsListOut = GetAllNonConditionalPromptsOnly(student.PINCL.P_INCL, inclusionReasonId);
                    break;
 
                //Reason 323
                case ((int)ReasonsForAdjustment.ResultsBelongToAnotherPupilKS3):
                    promptsListOut = GetAllPrompts(student.PINCL.P_INCL, inclusionReasonId);
                    break;

                //Reason 324
                case ((int)ReasonsForAdjustment.CancelAddBackKS5):
                    promptsListOut = GetAllPrompts(student.PINCL.P_INCL, inclusionReasonId);
                    break;

                default:
                    break;

            }

            return new AdjustmentOutcome(ConvertFromDto(promptsListOut));
        }

        private List<BusinessLogic.Entities.Prompt> ConvertFromDto(List<Prompt> promptsDto)
        {
            return promptsDto.Select(x => new BusinessLogic.Entities.Prompt
            {
                ColumnType = x.ColumnName,
                AllowNulls = x.AllowNulls,
                PromptID = x.PromptId,
                PromptShortText = x.PromptShortText,
                PromptText = x.PromptText,
                PromptType = GetPromptType(x.PromptTypesPromptType),
            }).ToList();
        }

        private PromptType GetPromptType(Infrastructure.SqlServer.DTOs.PromptType promptType)
        {
            switch (promptType.PromptTypeName)
            {
                case "ListBox":
                    return PromptType.ListBox;
                case "Date":
                    return PromptType.Date;
                case "Integer":
                    return PromptType.Integer;
                case "Text":
                    return PromptType.Text;
                case "YesNo":
                    return PromptType.YesNo;
                case "Info":
                    return PromptType.Info;
            }

            return PromptType.Text;
        }
        
        private List<Prompt> GetAllNonConditionalPromptsOnly(string pincl, int inclusionReasonId)
        {
            var query = GetAllPINCLInclusionAdjustments(pincl, inclusionReasonId);

            var inclusionReason = query.Select(ir => ir).ToList();

            PinclinclusionAdjustment pinclInclusionAdjustments = inclusionReason[0];
            return pinclInclusionAdjustments.PinclinclusionAdjData.Where(p => p.PromptsPrompt.IsConditional == false).Select(p => p.PromptsPrompt).ToList();
        }

        private List<Prompt> GetAllPrompts(string pincl, int inclusionReasonId)
        {
            //Retrieve the relevant prompts.
            var query = GetAllPINCLInclusionAdjustments(pincl, inclusionReasonId);

            var inclusionReason = query.Select(ir => ir).ToList();

            if (inclusionReason.Any())
            {
                PinclinclusionAdjustment pinclInclusionAdjustments = inclusionReason[0];
                return pinclInclusionAdjustments.PinclinclusionAdjData.Select(p => p.PromptsPrompt).ToList();
            }

            return new List<Prompt>();
        }

        private IQueryable<PinclinclusionAdjustment> GetAllPINCLInclusionAdjustments(string pincl, int inclusionReasonId)
        {
            return _repository.Get<PinclinclusionAdjustment>()
                .Where(pia => pia.PIncl == pincl && pia.IncAdjReason.IncAdjReasonId == inclusionReasonId)
                .Select(pia => pia);
        }

        private Prompt GetPromptByPromptId(int promptId)
        {
            return _repository
                .Get<Prompt>()
                .First(p => p.PromptId == promptId);
        }

        private string GetListItemPromptDisplayText(PromptAnswer answer)
        {
            short promptId = short.Parse(answer.PromptID.ToString());
            short selectedValue = short.Parse(answer.PromptSelectedValueAnswer);

            return _repository.Get<PromptResponse>()
                .Where(pr => pr.PromptId == promptId && pr.ListOrder == selectedValue)
                .Select(pr => pr.ListValue)
                .First();
        }

        /// <summary>
        /// Retrieve prompt 20. This is cross referenced by multiple reasons.
        /// </summary>
        /// <param name="context">The Web09_Entities context object to use to 
        /// retrieve the Exceptional Circumstances prompt</param>
        /// <returns>The Exceptional Circumstances prompt</returns>
        private Prompt GetExceptionalCircumstancesPrompt()
        {
            return _repository.Get<Prompt>()
                .Include("PromptTypes")
                .FirstOrDefault(p => p.PromptId == (int)Constants.PROMPT_ID_EXCEPTIONALCIRCUMSTANCES);
        }
    }
}
