using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSCohortAdjustmentRequests : Logic.TSBase
    {
        public static string GetDCSFNotification(int reasonID, bool isPLASC, int keystage, string type)
        {
            using (var conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (var context = new Web09_Entities(conn))
                {
                    if (type == "NOR Update")
                    {
                        var sr =
                            context.SchoolReasons.Where(
                                rc => rc.SchoolReasonId == reasonID && (rc.KeyStage == keystage || keystage == 0 || keystage == 9))
                                .FirstOrDefault();

                        if (sr != null)
                            return sr.DefaultDecisionComment;
                    } else
                    {
                        ReasonCohorts reasonCohort =
                            context.ReasonCohorts.Where(
                                rc => rc.ReasonID == reasonID && rc.IsPLASC == isPLASC && rc.KeyStage == keystage).
                                FirstOrDefault();

                        if (reasonCohort != null)
                            return reasonCohort.DefaultDecisionComment;                        
                    }

                    return string.Empty;
                }
            }
        }
    }
}
