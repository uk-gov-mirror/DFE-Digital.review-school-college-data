using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSSchool : Logic.TSBase
    {

        public static SchoolRequestChanges GetSchoolNORAdjustment(int dfesNumber)
        {

            using (var conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (var context = new Web09_Entities(conn))
                {

                    var req = context.SchoolRequestChanges
                        .Include("SchoolRequests")
                        .Include("SchoolRequests.Schools")
                        .Include("SchoolRequests.Changes")
                        .Include("Changes")
                        .Include("ScrutinyStatus")
                        .Where(src => src.SchoolRequests.Schools.DFESNumber == dfesNumber
                                      && src.DateEnd == null
                                      &&
                                      src.ScrutinyStatus.ScrutinyStatusCode !=
                                      Contants.SCRUTINY_STATUS_CANCELLED
                        );
                    if(req.Count() > 0)
                    {
                        return req.OrderByDescending(ob => ob.SchoolRequestID).FirstOrDefault();
                    }
                    return null;

                }
            }
        }

    }
}
