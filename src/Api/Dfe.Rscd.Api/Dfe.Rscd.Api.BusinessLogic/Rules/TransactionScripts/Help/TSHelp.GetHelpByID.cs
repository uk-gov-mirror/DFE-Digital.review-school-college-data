using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSHelp : Logic.TSBase
    {
        public static Help GetHelpByID(Int16 helpID)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    List<Help> list = context.Help.Include("Pages").Include("Cohorts").Include("SchoolGroups").Include("HelpUserLevels").Where(f => f.HelpID == helpID).ToList();                    
                    if (list.Count > 0)
                        return list[0];
                    else
                        throw Web09Exception.GetBusinessException(Web09MessageList.HelpInvalidID);
                }
            }
        }
    }
}
