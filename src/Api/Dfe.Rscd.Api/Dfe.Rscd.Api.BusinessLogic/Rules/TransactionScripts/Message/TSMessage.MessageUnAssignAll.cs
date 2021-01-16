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
        public static Int16 MessageUnAssignAll(Int16 messageID)
        {
            Int16 returnValue = 0;
            using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope())
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        List<Messages> saveMessage= null;
                        saveMessage = context.Messages.Include("Schools").Where(h => h.MessageID == messageID).ToList();

                        if(saveMessage.Count<=0)
                            throw Web09Exception.GetBusinessException(Web09MessageList.MessageInvalidID);

                        saveMessage[0].Schools.Clear();
                        context.ApplyPropertyChanges("Messages", saveMessage[0]);

                        context.SaveChanges();                        
                    }
                }
                transaction.Complete();
                return returnValue;
            }
        }
    }
}
