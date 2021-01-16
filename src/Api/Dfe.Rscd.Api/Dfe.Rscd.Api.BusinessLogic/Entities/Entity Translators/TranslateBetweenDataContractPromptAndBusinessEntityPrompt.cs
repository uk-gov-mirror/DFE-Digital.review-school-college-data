using System.Collections.Generic;
using System.Linq;

using Web09.Checking.Business.Logic.Entities;
using Web09.Services.DataContracts;
using Web09.Services.MessageContracts;

namespace Web09.Services.Adapters.EntityTranslators
{
    public class TranslateBetweenDataContractPromptAndBusinessEntityPrompt
    {
        public static GetAdjustmentPromptsResponse TranslateBusinessEntityAdjustmentPromptsToDataContractAdjustmentPrompts(AdjustmentFurtherPrompts adjPromptsIn)
        {
            GetAdjustmentPromptsResponse adjPromptsOut = new GetAdjustmentPromptsResponse();
            adjPromptsOut.PromptList = TranslateBetweenDataContractPromptAndBusinessEntityPrompt
                    .TranslateBusinessEntityPromptListToDataContractPromptList(adjPromptsIn.FurtherPrompts);
            adjPromptsOut.IsNonAdjustment = adjPromptsIn.IsNonAdjustment;
            return adjPromptsOut;
        }


        public static Web09.Services.DataContracts.PromptList TranslateBusinessEntityPromptListToDataContractPromptList(List<Web09.Checking.DataAccess.Prompts> promptListIn)
        {
            PromptList promptListOut = new PromptList();
            promptListOut.AddRange(promptListIn.ConvertAll(p => TranslateBusinessEntityPromptToDataContractPrompt(p)));
            return promptListOut;
        }

        public static Web09.Services.DataContracts.Prompt TranslateBusinessEntityPromptToDataContractPrompt(Web09.Checking.DataAccess.Prompts promptIn)
        {
            Web09.Services.DataContracts.Prompt promptOut = new Web09.Services.DataContracts.Prompt();
            promptOut.PromptID = promptIn.PromptID;
            promptOut.PromptText = promptIn.PromptText;
            promptOut.PromptShortText = promptIn.PromptShortText;
            
            promptOut.AllowNulls = promptIn.AllowNulls;
            promptOut.ColumnType = promptIn.ColumnName;
            switch(promptIn.PromptTypes.PromptTypeName)
            {
                case("ListBox"):
                    promptOut.PromptType = PromptType.ListBox;
                    break;
                case("Date"):
                    promptOut.PromptType = PromptType.Date;
                    break;
                case("Integer"):
                    promptOut.PromptType = PromptType.Integer;
                    break;
                case("Text"):
                    promptOut.PromptType = PromptType.Text;
                    break;
                case("YesNo"):
                    promptOut.PromptType = PromptType.YesNo;
                    break;
                case("Info"):
                    promptOut.PromptType = PromptType.Info;
                    break;
            }

            if (promptIn.PromptResponses.Count > 0)
                promptOut.PromptItemList = TranslateBusinessEntityPromptResponseListToDataContractPromptItemList(promptIn.PromptResponses.ToList());

            return promptOut;
        }

        public static Web09.Services.DataContracts.PromptItemList TranslateBusinessEntityPromptResponseListToDataContractPromptItemList(List<Web09.Checking.DataAccess.PromptResponses> promptItemListIn)
        {
            Web09.Services.DataContracts.PromptItemList promptItemListOut = new Web09.Services.DataContracts.PromptItemList();
            promptItemListOut.AddRange(promptItemListIn.ConvertAll(pi => TranslateBusinessEntityPromptResponseToDataContractPromptItem(pi)));
            return promptItemListOut;
        }

        public static Web09.Services.DataContracts.PromptItem TranslateBusinessEntityPromptResponseToDataContractPromptItem(Web09.Checking.DataAccess.PromptResponses promptItemIn)
        {
            Web09.Services.DataContracts.PromptItem promptItemOut = new Web09.Services.DataContracts.PromptItem();
            promptItemOut.PromptItemOrder = promptItemIn.ListOrder;
            promptItemOut.PromptItemValue = promptItemIn.ListValue;
            return promptItemOut;
        }
    }
}
