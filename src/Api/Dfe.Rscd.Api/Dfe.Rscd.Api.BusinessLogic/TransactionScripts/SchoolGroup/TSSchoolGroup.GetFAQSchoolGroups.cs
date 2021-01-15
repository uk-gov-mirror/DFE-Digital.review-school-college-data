using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSSchoolGroup : Logic.TSBase
    {
        /*this is returning a filtered list on IsManageOption is true */
        public static List<SchoolGroups> GetFAQSchoolGroups()
        {
            try
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();
                    
                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        return GetFAQSchoolGroups(context);
                      
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<SchoolGroups> GetFAQSchoolGroups(Web09_Entities context)
        {
            try
            {
                return context.SchoolGroups.Include("SchoolGroupTypes").OrderBy(r => r.SchoolGroupName).Where(r => r.IsManageOption ==true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
