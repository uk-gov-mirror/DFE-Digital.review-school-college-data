using System;
using Web09.Services.DataContracts;

namespace Web09.Services.Adapters.EntityTranslators
{
    public class TranslateBetweenDataContractPromptAnswersAndBusinessEntityPromptAnswers
    {
        public static Web09.Checking.Business.Logic.Entities.PromptAnswerList TranslateDataContractPromptAnswerListToBusinessEntityPromptAnswerList(DataContracts.PromptAnswerList answerListIn)
        {
            Web09.Checking.Business.Logic.Entities.PromptAnswerList answerListOut = new Web09.Checking.Business.Logic.Entities.PromptAnswerList();
            answerListOut.AddRange((answerListIn.ConvertAll(a => TranslateDataContractPromptAnswerToBusinessEntityPromptAnswer(a))));
            return answerListOut;
        }


        public static Web09.Checking.Business.Logic.Entities.PromptAnswer TranslateDataContractPromptAnswerToBusinessEntityPromptAnswer(DataContracts.PromptAnswer answerIn)
        {
            Web09.Checking.Business.Logic.Entities.PromptAnswer answerOut = new Web09.Checking.Business.Logic.Entities.PromptAnswer(answerIn.Prompt.PromptID);

            answerOut.PromptText = answerIn.Prompt.PromptText;
            answerOut.PromptShortText = answerIn.Prompt.PromptShortText;
            switch (answerIn.Prompt.PromptType)
            {
                case (PromptType.Date):
                    answerOut.PromptAnswerType = Web09.Checking.Business.Logic.Entities.PromptAnswer.PromptAnswerTypeEnum.Date;
                    answerOut.PromptDateTimeAnswer = answerIn.PromptDateTimeAnswer;
                    break;
                case (PromptType.Info):
                    answerOut.PromptAnswerType = Web09.Checking.Business.Logic.Entities.PromptAnswer.PromptAnswerTypeEnum.Info;
                    answerOut.PromptAcknowledgeInfoSightAnswer = answerIn.PromptInfoAcknowledgeSeen;
                    break;
                case (PromptType.Integer):
                    answerOut.PromptAnswerType = Web09.Checking.Business.Logic.Entities.PromptAnswer.PromptAnswerTypeEnum.Integer;
                    answerOut.PromptIntegerAnswer = answerIn.PromptIntegerAnswer;
                    break;
                case (PromptType.ListBox):
                    answerOut.PromptAnswerType = Web09.Checking.Business.Logic.Entities.PromptAnswer.PromptAnswerTypeEnum.ListBox;
                    answerOut.PromptSelectedValueAnswer = answerIn.PromptSelectedValueAnswer;
                    break;
                case (PromptType.Text):
                    answerOut.PromptAnswerType = Web09.Checking.Business.Logic.Entities.PromptAnswer.PromptAnswerTypeEnum.Text;
                    answerOut.PromptStringAnswer = answerIn.PromptStringAnswer;
                    break;
                case (PromptType.YesNo):
                    answerOut.PromptAnswerType = Web09.Checking.Business.Logic.Entities.PromptAnswer.PromptAnswerTypeEnum.YesNo;
                    answerOut.PromptYesNoAnswer = answerIn.PromptYesNoAnswer;
                    break;
                default: 
                    break;
            }

            answerOut.AllowNulls = answerIn.Prompt.AllowNulls;
            answerOut.ColumnType = answerIn.Prompt.ColumnType;
            answerOut.WarningMessage = answerIn.WarningMessage;
            answerOut.ErrorMessage = answerIn.ErrorMessage;
            answerOut.InformationMessage = answerIn.InformationMessage;

            return answerOut;
        }

        public static Web09.Services.DataContracts.PromptAnswerList TranslateBusinessEntityPromptAnswerListToDataContractPromptAnswerList(Web09.Checking.Business.Logic.Entities.PromptAnswerList answerListIn)
        {
            Web09.Services.DataContracts.PromptAnswerList answerListOut = new Web09.Services.DataContracts.PromptAnswerList();
            answerListOut.AddRange((answerListIn.ConvertAll(a => TranslateBusinessEntityPromptAnswerToDataContractPromptAnswer(a))));
            return answerListOut;
        }

        public static Web09.Services.DataContracts.PromptAnswer TranslateBusinessEntityPromptAnswerToDataContractPromptAnswer(Web09.Checking.Business.Logic.Entities.PromptAnswer answerIn)
        {
            Web09.Services.DataContracts.PromptAnswer answerOut = new Web09.Services.DataContracts.PromptAnswer();

            answerOut.Prompt = new Web09.Services.DataContracts.Prompt();
            answerOut.Prompt.PromptID = answerIn.PromptID;

            answerOut.Prompt.PromptText = answerIn.PromptText;
            answerOut.Prompt.PromptShortText = answerIn.PromptShortText;
            answerOut.Prompt.UpdatedByDA = (Int16) (answerIn.UpdatedByDA.HasValue==false || answerIn.UpdatedByDA.Value==0? 0: 1);

            switch (answerIn.PromptAnswerType)
            {
                case (Web09.Checking.Business.Logic.Entities.PromptAnswer.PromptAnswerTypeEnum.Date):
                    answerOut.Prompt.PromptType = Web09.Services.DataContracts.PromptType.Date;
                    answerOut.PromptDateTimeAnswer = answerIn.PromptDateTimeAnswer;
                    break;
                case (Web09.Checking.Business.Logic.Entities.PromptAnswer.PromptAnswerTypeEnum.Info):
                    answerOut.Prompt.PromptType = Web09.Services.DataContracts.PromptType.Info;
                    answerOut.PromptInfoAcknowledgeSeen = answerIn.PromptAcknowledgeInfoSightAnswer;
                    break;
                case (Web09.Checking.Business.Logic.Entities.PromptAnswer.PromptAnswerTypeEnum.Integer):
                    answerOut.Prompt.PromptType = Web09.Services.DataContracts.PromptType.Integer;
                    answerOut.PromptIntegerAnswer = answerIn.PromptIntegerAnswer;
                    break;
                case (Web09.Checking.Business.Logic.Entities.PromptAnswer.PromptAnswerTypeEnum.ListBox):
                    answerOut.Prompt.PromptType = Web09.Services.DataContracts.PromptType.ListBox;
                    answerOut.PromptSelectedValueAnswer = answerIn.PromptSelectedValueAnswer;
                    break;
                case (Web09.Checking.Business.Logic.Entities.PromptAnswer.PromptAnswerTypeEnum.Text):
                    answerOut.Prompt.PromptType = Web09.Services.DataContracts.PromptType.Text;
                    answerOut.PromptStringAnswer = answerIn.PromptStringAnswer;
                    break;
                case (Web09.Checking.Business.Logic.Entities.PromptAnswer.PromptAnswerTypeEnum.YesNo):
                    answerOut.Prompt.PromptType = Web09.Services.DataContracts.PromptType.YesNo;
                    answerOut.PromptYesNoAnswer = answerIn.PromptYesNoAnswer;
                    break;
                default:
                    break;
            }

            answerOut.WarningMessage = answerIn.WarningMessage;
            answerOut.ErrorMessage = answerIn.ErrorMessage;
            answerOut.InformationMessage = answerIn.InformationMessage;

            return answerOut;
        }
    }
}
