using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSMessage : Logic.TSBase
    {
        public static List<MessagesWrapper> GetMessageList(Int16? messageTypeID, Int16? cohortID, bool? june, bool? sept, bool? active, int? dfesNumber)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {

                    // Validation
                    if (cohortID.HasValue && !Validation.Common.IsKeyStageValid(context, cohortID.Value))
                        throw Web09Exception.GetBusinessException(Web09MessageList.KeystageInvalid);
                    if (messageTypeID.HasValue && !Validation.Message.IsMessageTypeIDValid(context, messageTypeID.Value))
                        throw Web09Exception.GetBusinessException(Web09MessageList.MessageInvalidMessageTypeID);

                    int vmessageTypeID = 0, vcohortID = 0;
                    int vdfesNumber = 0;

                    if (cohortID.HasValue)
                        vcohortID = cohortID.Value;                    
                    if (messageTypeID.HasValue)
                        vmessageTypeID = messageTypeID.Value;
                    if (dfesNumber.HasValue)
                        vdfesNumber = dfesNumber.Value;

                    return
                    (
                        from f
                        in context.Messages
                            .Include("MessageTypes")
                            .Include("Cohorts")
                            .Include("Schools")
                        where
                            (f.Cohorts.Any(c => c.KeyStage == vcohortID) || vcohortID == 0)
                            && (f.MessageTypes.MessageTypeID == vmessageTypeID || vmessageTypeID == 0)
                            && (f.Schools.Any(s => s.DFESNumber == vdfesNumber || vdfesNumber == 0))
                            && (!sept.HasValue || sept.Value == f.IsSept)
                            && (!june.HasValue || june.Value == f.IsJune)
                            && (!active.HasValue || active.Value == f.IsActive)
                        group f by new
                        {
                            MessageID = f.MessageID,
                            MessageText = f.MessageText,
                            MessageTypes = f.MessageTypes,
                            IsActive = f.IsActive,
                            IsJune = f.IsJune,
                            IsSept = f.IsSept,
                            SchoolCount = f.Schools.Count
                        }
                            into messagesGroup                            
                            select new
                            {
                                Value = new
                                {
                                    MessageID = messagesGroup.Key.MessageID,
                                    MessageText = messagesGroup.Key.MessageText,
                                    MessageTypes = messagesGroup.Key.MessageTypes,
                                    IsActive = messagesGroup.Key.IsActive,
                                    IsJune = messagesGroup.Key.IsJune,
                                    IsSept = messagesGroup.Key.IsSept
                                },
                                SchoolCount = messagesGroup.Key.SchoolCount
                            }
                    )
                    .ToList()
                    .ConvertAll<MessagesWrapper>
                    (
                        x =>
                            new MessagesWrapper
                            {
                                Value = new Messages
                                {
                                    IsActive = x.Value.IsActive,
                                    IsJune = x.Value.IsJune,
                                    IsSept = x.Value.IsSept,
                                    MessageID = x.Value.MessageID,
                                    MessageText = x.Value.MessageText,
                                    MessageTypes = x.Value.MessageTypes
                                },
                                SchoolCount = x.SchoolCount
                            }
                    )
                    .OrderBy(x => x.Value.MessageID)
                    .ToList();
                }
            }
        }
    }
}
