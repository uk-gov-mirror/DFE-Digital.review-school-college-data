using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;


namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSPage : Logic.TSBase
    {
        public static List<Pages> GetPageList()
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return context.Pages.OrderBy(r => r.PageName).ToList();
                }
            }
        }
    }
}
