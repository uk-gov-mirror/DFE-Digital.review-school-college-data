using System;
using System.Collections.Generic;
using System.Text;

namespace Dfe.Rscd.Api.BusinessLogic.Entities
{
    public class CompleteSimpleOutcomeCheck
    {
        public OutcomeStatus OutcomeStatus { get;set; }

        public CompleteSimpleOutcomeCheck(OutcomeStatus outcomeStatus)
        {
            OutcomeStatus = outcomeStatus;
        }
    }
}
