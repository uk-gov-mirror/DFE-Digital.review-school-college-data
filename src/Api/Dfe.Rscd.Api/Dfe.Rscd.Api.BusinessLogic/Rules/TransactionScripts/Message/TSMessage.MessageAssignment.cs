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
        public static Int16 MessageAssignment(Int16 messageID, List<int> assignSchools, List<int> unAssignSchools)
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
                        
                        if(!Validation.Message.IsMessageIDValid(context,messageID))
                            throw Web09Exception.GetBusinessException(Web09MessageList.MessageInvalidID);

                        saveMessage = context.Messages.Include("Schools").Where(h => h.MessageID == messageID).First();

                        foreach (int id in assignSchools)
                        {
                            List<Schools> sList = (from s in context.Schools where s.DFESNumber == id select s).ToList();
                            if(sList.Count<=0)
                                throw Web09Exception.GetBusinessException(Web09MessageList.DCSFNumberInvalid);
                            else
                            saveMessage.Schools.Add(sList[0]);
                        }

                        foreach (int id in unAssignSchools)
                        {
                            List<Schools> sList = (from s in context.Schools where s.DFESNumber == id select s).ToList();
                            if (sList.Count <= 0)
                                throw Web09Exception.GetBusinessException(Web09MessageList.DCSFNumberInvalid);
                            else
                                saveMessage.Schools.Remove(sList[0]); 
                        }
                        
                        context.ApplyPropertyChanges("Messages", saveMessage);

                        context.SaveChanges();                        
                    }
                }
                transaction.Complete();
                return returnValue;
            }
        }
    }
}
