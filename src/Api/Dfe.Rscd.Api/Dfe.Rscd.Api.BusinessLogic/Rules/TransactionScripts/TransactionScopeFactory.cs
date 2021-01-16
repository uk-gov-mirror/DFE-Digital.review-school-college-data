using System;
using System.Configuration;
using System.Transactions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    // TFS 20825
    class TransactionScopeFactory
    {
        // Never use "new TransactionScope()" as it setsup SQL isolation level of serializable by default        
        public static TransactionScope CreateTransactionScope()
        {
            var transactionOptions            = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadCommitted;

            bool timeoutset = false;
            string appSetting = ConfigurationManager.AppSettings["transactionscopetimeoutseconds"];
            if (!string.IsNullOrEmpty(appSetting))
            {
                int transactionScopeTimeoutseconds;
                if (Int32.TryParse(appSetting, out transactionScopeTimeoutseconds))
                {
                    TimeSpan timespan = TimeSpan.FromSeconds(transactionScopeTimeoutseconds);
                    transactionOptions.Timeout = timespan;
                    timeoutset = true;
                }
            }

            if (!timeoutset)
            {
                transactionOptions.Timeout = TimeSpan.FromSeconds(30);
            }

            return new TransactionScope(TransactionScopeOption.Required, transactionOptions);
        }
    }
}
