using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSFAQ : Logic.TSBase
    {
        public static FAQ GetFAQByID(Int16 faqID)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    List<FAQ> list = context.FAQ.Include("FAQCategories").Include("Cohorts").Include("SchoolGroups").Where(f => f.FAQID == faqID).ToList();

                    if (list.Count > 0)
                        return list[0];
                    else
                        throw Web09Exception.GetBusinessException(Web09MessageList.FAQInvalidID);
                }
            }
        }
    }
}
