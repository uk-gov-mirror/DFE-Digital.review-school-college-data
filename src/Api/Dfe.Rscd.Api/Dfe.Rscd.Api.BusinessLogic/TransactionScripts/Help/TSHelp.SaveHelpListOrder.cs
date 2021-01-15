using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Transactions;
using Web09.Checking.DataAccess;


namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSHelp : Logic.TSBase
    {
        public static void SaveHelpListOrder(List<Help> helpList)
        {
            using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope())
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        Help saveHelp = null;

                        foreach (Help help in helpList)
                        {
                            saveHelp = context.Help.Where(f => f.HelpID == help.HelpID).First();
                            saveHelp.ListOrder = help.ListOrder;
                            context.ApplyPropertyChanges("Help", saveHelp);
                        }

                        context.SaveChanges();
                    }
                }
                transaction.Complete();
            }
        }
    }
}
