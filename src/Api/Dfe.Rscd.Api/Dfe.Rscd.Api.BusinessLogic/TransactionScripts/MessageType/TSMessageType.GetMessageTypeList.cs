using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;


namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSMessageType : Logic.TSBase
    {
        public static List<MessageTypes> GetMessageTypeList()
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return context.MessageTypes.OrderBy(r => r.MessageTypeName).ToList<MessageTypes>();
                }
            }
        }
    }
}
