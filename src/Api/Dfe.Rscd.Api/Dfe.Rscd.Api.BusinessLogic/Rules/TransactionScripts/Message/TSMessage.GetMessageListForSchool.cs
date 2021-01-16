using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSMessage : Logic.TSBase
    {
        public static List<Messages> GetMessageListForSchool(int dfesNumber)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {

                    // Validation
                    if (dfesNumber>0 && !Validation.Common.IsDCSFNumberValid(context, dfesNumber))
                        throw Web09Exception.GetBusinessException(Web09MessageList.DCSFNumberInvalid);                   

                    var query = (from f in context.Messages
                                     .Include("MessageTypes")
                                     .Include("Cohorts")
                                     .Include("Schools")
                                 where (f.Schools.Any(s => s.DFESNumber == dfesNumber) && f.IsActive == true)                                 
                                 select new { MESSAGE = f, MESSAGETYPE = f.MessageTypes }
                                       ).OrderBy(o => o.MESSAGE.MessageID).ToList();

                    List<Messages> list = new List<Messages>();
                    foreach (var item in query)
                    {                        
                        Messages message = item.MESSAGE;                        
                        message.MessageTypes = new MessageTypes { MessageTypeID = item.MESSAGE.MessageTypes.MessageTypeID, MessageTypeName = item.MESSAGE.MessageTypes.MessageTypeName };
                        list.Add(message);
                    }
                    return list;
                }
            }
        }
    }
}
