using System;
using System.Collections.Generic;
using System.Linq;

using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {
        private static List<Prompts> GetAdjustmentPrompts_PupilNotAtEndOfKeyStage3InAllSubjects(Web09_Entities context, Students student)
        {
            List<Prompts> promptListOut = new List<Prompts>();

            StudentChanges studentChange = student.StudentChanges.First();

            if (IsStudentYearGroupLessThanValue(studentChange.YearGroups.YearGroupCode, 9))
            {
                if (!String.IsNullOrEmpty(studentChange.AMDFlag) && studentChange.AMDFlag.ToLower() == "z")
                {
                    //Prompt 31310 is required
                    promptListOut = context.Prompts
                        .Include ("PromptTypes")
                        .Where(p => p.PromptID == 31310)
                        .Select(p => p).ToList();
                }
                else
                {
                    //Prompt 31330 required
                    promptListOut = context.Prompts
                        .Include("PromptTypes")
                        .Where(p => p.PromptID == 31330)
                        .Select(p => p).ToList();
                }
            }
            else
            {
                //Prompt 31320 required
                promptListOut = context.Prompts
                        .Include("PromptTypes")
                        .Where(p => p.PromptID == 31320)
                        .Select(p => p).ToList();
            }

            return promptListOut;
        }

        //private static bool IsYearGroupLessThanNine(string yearGroupCode)
        //{
        //    int yearGroup;
        //    if(int.TryParse(yearGroupCode, out yearGroup))
        //    {
        //        return (yearGroup < 9) ? true : false;
        //    }
        //    else
        //    {
        //        return false;
        //    }
            
        //}
    }

    
}
