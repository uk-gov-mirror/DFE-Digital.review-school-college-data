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

        public AmendmentOutcome GetAdjustmentPrompts(CheckingWindow checkingWindow, string pinclCode, int inclusionReasonId)
        {
            _checkingWindow = checkingWindow;

            return GetAdjustmentPrompts(pinclCode, inclusionReasonId);
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

        private AmendmentOutcome GetAdjustmentPrompts(string pinclCode, int inclusionReasonId)
        {
            
            List<Prompt> promptsListOut = new List<Prompt>();

            switch (inclusionReasonId)
            {

                //Reason 3
                case ((int)ReasonsForAdjustment.CancelAddBack):
                    promptsListOut = GetAllPrompts(pinclCode, inclusionReasonId);
                    break;
                //Reason 4
                case ((int)ReasonsForAdjustment.CancelAdjustmentForAdmissionFollowingPermanentExclusion):
                    promptsListOut = GetAllPrompts(pinclCode, inclusionReasonId);
                    break;
                //Reason 5
                case ((int)ReasonsForAdjustment.RemovePupilCompletelyFromAllData):
                    promptsListOut = GetAllPrompts(pinclCode, inclusionReasonId);
                    break;
                //Reason 6
                case ((int)ReasonsForAdjustment.PublishPupilAtThisSchool):
                    promptsListOut = GetAllPrompts(pinclCode, inclusionReasonId);
                    break;
                //Reason 7
                case ((int)ReasonsForAdjustment.ReinstateThePupilKS4):
                    promptsListOut = GetAllPrompts(pinclCode, inclusionReasonId);
                    break;

                //Reason 8
                case ((int)ReasonsForAdjustment.AdmittedFromAbroad):
                    promptsListOut = AdmittedFromAbroad(pinclCode, inclusionReasonId);
                    break;

                //Reason 9
                case ((int)ReasonsForAdjustment.ContingencyKS4):
                    promptsListOut = GetAllPrompts(pinclCode, inclusionReasonId);
                    break;

                //Reason 10
                case ((int)ReasonsForAdjustment.AdmittedFollowingPermanentExclusionFromMaintainedSchool):
                    promptsListOut = GetAllNonConditionalPromptsOnly(pinclCode, inclusionReasonId);
                    break;

                //Reason 11
                case ((int)ReasonsForAdjustment.PermanentlyLeftEngland):
                    promptsListOut = GetAllNonConditionalPromptsOnly(pinclCode, inclusionReasonId);
                    break;
                
                //Reason 12
                case ((int)ReasonsForAdjustment.Deceased):
                    promptsListOut = GetAllNonConditionalPromptsOnly(pinclCode, inclusionReasonId);
                    break;

                //Reason 17
                case ((int)ReasonsForAdjustment.PupilAtEndOfKeyStage4):
                    promptsListOut.Add(GetPromptByPromptId(1700));
                    promptsListOut.Add(GetPromptByPromptId((int)Constants.PROMPT_ID_NC_YEAR_GROUP_KS4));
                    break;

                //Reason 18 
                case ((int)ReasonsForAdjustment.LeftSchoolRollBeforeExamsKS4):
                    promptsListOut = GetAllNonConditionalPromptsOnly(pinclCode, inclusionReasonId);
                    break;

                //Reason 19 
                case ((int)ReasonsForAdjustment.KS4Other):
                    promptsListOut = GetAllNonConditionalPromptsOnly(pinclCode, inclusionReasonId);
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
                    promptsListOut = GetAllPrompts(pinclCode, inclusionReasonId);
                    break;
                
                //Reason 55 
                case ((int)ReasonsForAdjustment.LeftBeforeExamsKS5):
                    promptsListOut = GetAllPrompts(pinclCode, inclusionReasonId);
                    break;
                
                //Reason 56 
                case ((int)ReasonsForAdjustment.KS5Other):
                    promptsListOut = GetAllNonConditionalPromptsOnly(pinclCode, inclusionReasonId);
                    break;
                
                //Reason 58 
                case ((int)ReasonsForAdjustment.ResultsBelongToAnotherPupilKS5):
                    promptsListOut = GetAllPrompts(pinclCode, inclusionReasonId);
                    break;
                
                //Reason 59 
                case ((int)ReasonsForAdjustment.ReinstatePupilKS5):
                    promptsListOut = GetAllPrompts(pinclCode, inclusionReasonId);
                    break;

                //Reason 212
                case ((int)ReasonsForAdjustment.ContingencyKS2):
                    promptsListOut = GetAllPrompts(pinclCode, inclusionReasonId);
                    break;

                //Reason 218
                case ((int)ReasonsForAdjustment.LeftSchoolRollBeforeTestsKS2):
                    promptsListOut = GetAllNonConditionalPromptsOnly(pinclCode, inclusionReasonId);
                    break;

                //Reason 219
                case ((int)ReasonsForAdjustment.KS2Other):
                    promptsListOut = GetAllPrompts(pinclCode, inclusionReasonId);
                    break;

                //Reason 223
                case ((int)ReasonsForAdjustment.ResultsBelongToAnotherPupilKS2):
                    promptsListOut = GetAllPrompts(pinclCode, inclusionReasonId);
                    break;

                //Reason 318
                case ((int)ReasonsForAdjustment.LeftSchoolRollBeforeTestsKS3):
                    promptsListOut = GetAllNonConditionalPromptsOnly(pinclCode, inclusionReasonId);
                    break;

                //Reason 319
                case ((int)ReasonsForAdjustment.KS3Other):
                    promptsListOut = GetAllNonConditionalPromptsOnly(pinclCode, inclusionReasonId);
                    break;
 
                //Reason 323
                case ((int)ReasonsForAdjustment.ResultsBelongToAnotherPupilKS3):
                    promptsListOut = GetAllPrompts(pinclCode, inclusionReasonId);
                    break;

                //Reason 324
                case ((int)ReasonsForAdjustment.CancelAddBackKS5):
                    promptsListOut = GetAllPrompts(pinclCode, inclusionReasonId);
                    break;

                default:
                    break;

            }

            return new AmendmentOutcome(ConvertFromDto(promptsListOut));
        }

        private List<BusinessLogic.Entities.Prompt> ConvertFromDto(List<Prompt> promptsDto)
        {
            var prompts = promptsDto.Select(x => new BusinessLogic.Entities.Prompt
            {
                ColumnType = x.ColumnName,
                AllowNulls = x.AllowNulls,
                PromptID = x.PromptId,
                PromptShortText = x.PromptShortText,
                PromptText = x.PromptText,
                PromptType = x.PromptTypesPromptType.PromptTypeName,
                PromptItemList = x.PromptResponses.Select(x=> new PromptItem
                {
                    PromptItemOrder = x.ListOrder,
                    PromptItemValue = x.ListValue 
                }).ToList()
            }).ToList();

            if (prompts.Count > 0)
                return prompts;

            return null;
        }

       private List<Prompt> GetAllNonConditionalPromptsOnly(string pincl, int inclusionReasonId)
        {
            var prompts = _repository.Get<Prompt>().Where(x =>
                x.IsConditional == false && x.PinclinclusionAdjData.Any(j =>
                    j.PinclinclusionAdjustmentsIncAdjReasonId == inclusionReasonId &&
                    j.PinclinclusionAdjustmentsPIncl == pincl))
                .Include(x=>x.PromptResponses)
                .Include(x => x.PromptTypesPromptType)
                .ToList();

            return prompts;
        }

        private List<Prompt> GetAllPrompts(string pincl, int inclusionReasonId)
        {
            var prompts = _repository.Get<Prompt>().Where(x => 
                    x.PinclinclusionAdjData.Any(j =>
                        j.PinclinclusionAdjustmentsIncAdjReasonId == inclusionReasonId &&
                        j.PinclinclusionAdjustmentsPIncl == pincl))
                .Include(x=>x.PromptResponses)
                .Include(x => x.PromptTypesPromptType)
                .ToList();

            return prompts;
        }

        private Prompt GetPromptByPromptId(int promptId)
        {
            return _repository
                .Get<Prompt>()
                .Include(x=>x.PromptResponses)
                .First(p => p.PromptId == promptId);
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
