using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSMessage : Logic.TSBase
    {
        public static Messages GetMessageByID(Int16 messageID, bool includeSchools)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    List<Messages> list;
                    if(includeSchools)
                        list = context.Messages.Include("Schools").Include("MessageTypes").Include("Cohorts").Where(f => f.MessageID == messageID).ToList();
                    else
                        list = context.Messages.Include("MessageTypes").Include("Cohorts").Where(f => f.MessageID == messageID).ToList();

                    if (list.Count > 0)
                        return list[0];
                    else
                        throw Web09Exception.GetBusinessException(Web09MessageList.MessageInvalidID);

                }
            }
        }
    }
}
