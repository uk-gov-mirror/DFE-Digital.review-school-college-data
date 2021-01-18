using System.Collections.Generic;
using Dfe.Rscd.Api.BusinessLogic.Common;
using Dfe.Rscd.Api.BusinessLogic.Entities;
using Prompt = Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs.Prompt;

namespace Dfe.Rscd.Api.Services
{
    public partial class RemovePupilPromptsService
    {

        private List<Prompt> GetAdjustmentPrompts_PupilNotAtEndOfKeyStage4(Pupil student)
        {
            //List<Prompts> promptListReturn = new List<Prompts>();

            //if (student == null || student.StudentChanges.Count == 0)
            //    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            //StudentChanges studentChange = student.StudentChanges.First();

            //if (studentChange == null || studentChange.YearGroups == null)
            //    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);
            

            //int yearGroup;
            //if (int.TryParse(studentChange.YearGroups.YearGroupCode, out yearGroup) && yearGroup == 11)
            //{
            //    //Return prompt 1320, and prompt for NC Year Group.
            //    promptListReturn.Add(GetPromptByPromptID(1320));
            //    promptListReturn.Add(GetPromptByPromptID(Constants.PROMPT_ID_NC_YEAR_GROUP_KS4));
            //}
            //else
            //{
            //    promptListReturn.Add(GetExceptionalCircumstancesPrompt(context));
            //}

            //return promptListReturn;

            return new List<Prompt>();
        }
    }
}
