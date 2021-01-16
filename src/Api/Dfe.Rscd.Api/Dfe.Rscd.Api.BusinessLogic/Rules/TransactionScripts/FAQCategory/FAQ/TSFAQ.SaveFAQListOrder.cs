using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Transactions;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSFAQ : Logic.TSBase
    {
        public static void SaveFAQListOrder(List<FAQ> faqs)
        {
            using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope())
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        FAQ saveFAQ = null;

                        foreach (FAQ faq in faqs)
                        {
                            if (!Validation.FAQ.IsFAQIDValid(context, faq.FAQID))
                                throw Web09Exception.GetBusinessException(Web09MessageList.FAQInvalidID);

                            saveFAQ = context.FAQ.Where(f => f.FAQID == faq.FAQID).First();
                            
                            saveFAQ.ListOrder = faq.ListOrder;
                            context.ApplyPropertyChanges("FAQ", saveFAQ);                            
                        }

                        context.SaveChanges();
                    }
                }
                transaction.Complete();
            }
        }
    }
}
