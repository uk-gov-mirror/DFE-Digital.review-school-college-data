using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Transactions;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSMessage : Logic.TSBase
    {
        public static Int16 SaveMessage(Messages message)
        {
            Int16 returnValue = 0;
            using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope() )
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        Messages saveMessage= null;

                        if (!Validation.Message.IsMessageRequiredInfoValid(context, message))
                            throw Web09Exception.GetBusinessException(Web09MessageList.MessageIncompleteInformation);

                        if (message.MessageID == 0) // new
                        {
                            saveMessage = new Messages();
                            saveMessage.Cohorts = new System.Data.Objects.DataClasses.EntityCollection<Cohorts>();
                        }
                        else
                        {
                             if (!Validation.Message.IsMessageIDValid(context, message.MessageID))
                                 throw Web09Exception.GetBusinessException(Web09MessageList.MessageInvalidID);

                            saveMessage = context.Messages.Include("MessageTypes").Include("Cohorts").Where(h => h.MessageID == message.MessageID).First();
                        }

                        saveMessage.MessageTypes = context.MessageTypes.Where(p => p.MessageTypeID == message.MessageTypes.MessageTypeID).ToList().First();

                        saveMessage.IsActive = message.IsActive;
                        saveMessage.IsJune = message.IsJune;
                        saveMessage.IsSept = message.IsSept;

                        saveMessage.MessageText = message.MessageText.Trim();

                        List<Cohorts> cohortList = context.Cohorts.ToList();                        

                        saveMessage.Cohorts.Clear();
                        if (message.Cohorts != null && message.Cohorts.Count > 0)
                            for (int counter = 0; counter < message.Cohorts.ToList().Count; counter++)
                                saveMessage.Cohorts.Add((from c in cohortList where c.KeyStage == message.Cohorts.ToList()[counter].KeyStage select c).First());
                       
                        if (saveMessage.MessageID == 0) // new message
                            context.AddToMessages(saveMessage);
                        else
                            context.ApplyPropertyChanges("Messages", saveMessage);

                        context.SaveChanges();

                        returnValue = saveMessage.MessageID;
                    }
                }
                transaction.Complete();
                return returnValue;
            }
        }
    }
}
