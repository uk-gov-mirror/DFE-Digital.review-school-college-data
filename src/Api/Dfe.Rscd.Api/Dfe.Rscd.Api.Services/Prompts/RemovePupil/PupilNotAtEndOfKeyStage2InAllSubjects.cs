using System.Collections.Generic;
using Dfe.Rscd.Api.BusinessLogic.Common;
using Dfe.Rscd.Api.BusinessLogic.Entities;
using Prompt = Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs.Prompt;

namespace Dfe.Rscd.Api.Services
{
    public partial class RemovePupilPromptsService
    {
        private List<Prompt> GetAdjustmentPrompts_PupilNotAtEndOfKeyStage2InAllSubjects(Pupil student)
        {
            //List<Prompts> promptListOut = new List<Prompts>();

            //if (student == null ||
            //    student.StudentChanges.First() == null ||
            //    student.StudentChanges.First().YearGroups == null ||
            //    student.StudentChanges.First().YearGroups.YearGroupCode == null ||
            //    student.PINCLs == null || student.PINCLs.P_INCL == null)
            //    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            //List<Prompts> furtherPrompts = new List<Prompts>();
            //int yearGroup;

            //if (student.PINCLs.P_INCL == "202")
            //{
            //    promptListOut.Add(GetPromptByPromptID(21310));
            //    promptListOut.Add(GetPromptByPromptID(Constants.PROMPT_ID_EXCEPTIONALCIRCUMSTANCES));
            //}
            //else if (int.TryParse(student.StudentChanges.First().YearGroups.YearGroupCode, out yearGroup))
            //{
            //    if (yearGroup >= 6)
            //    {
            //        Prompts prompt21320 = GetPromptByPromptID(21320);
            //        prompt21320.PromptText = prompt21320.PromptText.Replace("[Year Group]", yearGroup.ToString());
            //        promptListOut.Add(prompt21320);
            //    }
            //    else //ie year group < 6
            //    {
            //        promptListOut.Add(GetPromptByPromptID(21330));
            //    }
            //}
            //else
            //{
            //    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            //}

            //return promptListOut;

            return new List<Prompt>();
        }

    }
}
