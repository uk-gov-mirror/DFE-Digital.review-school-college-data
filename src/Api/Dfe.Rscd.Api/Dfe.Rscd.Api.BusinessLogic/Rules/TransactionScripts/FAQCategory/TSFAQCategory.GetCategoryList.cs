using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSFAQCategory : Logic.TSBase
    {
        public static List<FAQCategories> GetCategoryList()
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return context.FAQCategories.OrderBy(r=>r.ListOrder).ToList<FAQCategories>();
                }
            }
        }
    }
}
