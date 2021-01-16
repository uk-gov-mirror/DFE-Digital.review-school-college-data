using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;


namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSCohort : Logic.TSBase
    {
        public static List<Cohorts> GetList()
        {
            try
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        return GetList(context);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Cohorts> GetList(Web09_Entities context)
        {
            try
            {
                return context.Cohorts.OrderBy(r => r.KeyStageName).ToList<Cohorts>();                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
