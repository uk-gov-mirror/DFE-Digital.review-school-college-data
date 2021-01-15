using System.Collections.Generic;

using Web09.Checking.DataAccess;

namespace Web09.Services.Adapters.EntityTranslators
{
    public class TranslateBetweenDataContractPromptAndBusinessEntityStudentRequestData
    {
        public static List<StudentRequestData> TranslateDataContractListOfPromptsToBusinessEntityListOfStudentRequestData(Web09.Services.DataContracts.Prompts prompts)
        {
            List<StudentRequestData> returnValue = new List < StudentRequestData >(prompts.Count);
            returnValue.AddRange(prompts.ConvertAll(p => TranslateDataContractPromptToBusinessEntityStudentRequestData(p)));
            return returnValue;
        }

        public static StudentRequestData TranslateDataContractPromptToBusinessEntityStudentRequestData(Web09.Services.DataContracts.Prompt prompt)
        {
            return new StudentRequestData
            {
                //PromptID = prompt.Code,
                //PromptValue = prompt.Response                
            };
        }
    }
}
