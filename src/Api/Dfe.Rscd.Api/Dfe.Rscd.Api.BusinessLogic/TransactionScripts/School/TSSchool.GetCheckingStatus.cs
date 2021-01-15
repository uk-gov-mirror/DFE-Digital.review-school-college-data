using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSSchool : Logic.TSBase
    {
        public static SchoolCheckingStatus GetSchoolCheckingStatus(int dfesNumber, short keystage)
        {
            SchoolCheckingStatus status = null;

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    var query = context.SchoolCheckingStatus
                                .Include("Schools")
                                .Include("Cohorts")
                                .Include("CheckingStatus")
                                .Where(scs => scs.Schools.DFESNumber == dfesNumber
                                        && scs.Cohorts.KeyStage == keystage
                                        && ((scs.Schools.LowestAge < 16)
                                            || (scs.Cohorts.KeyStage == 5 && scs.Schools.LowestAge >= 16))).ToList();

                    if (query.Count > 0)
                    {
                        status = query.First();
                        status.ChangesReference.Load();                                                
                    }
                }
            }

            return status;
        }
    }
}
