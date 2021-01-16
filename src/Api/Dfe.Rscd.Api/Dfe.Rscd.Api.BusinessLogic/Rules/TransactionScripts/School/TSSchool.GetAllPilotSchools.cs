using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;

using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSSchool : Logic.TSBase
    {
        public static List<Schools> GetAllPilotSchools()
        {
            try
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {

                        bool isJuneChecking = TSResult.IsJuneChecking(context);                     

                        var query = (from sl in context.Schools
                                     where sl.SchoolGroups.Any(sg1 => sg1.SchoolGroupName.Equals("ReportCardPilot"))                                  
                                     select new { sl });

                        if (isJuneChecking)
                        {
                            query = query.Where(sl => sl.sl.SchoolGroups.Any(sg => sg.SchoolGroupName.Equals("Maintained") && sl.sl.SchoolCheckingStatus.Any(sc => sc.Cohorts.KeyStage == 4)));
                        }

                        var result = query.Distinct().ToList();

                        List<Schools> list = new List<Schools>();
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].sl.SchoolStatusReference.Load();
                            list.Add(result[i].sl);
                        }

                        return list;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
