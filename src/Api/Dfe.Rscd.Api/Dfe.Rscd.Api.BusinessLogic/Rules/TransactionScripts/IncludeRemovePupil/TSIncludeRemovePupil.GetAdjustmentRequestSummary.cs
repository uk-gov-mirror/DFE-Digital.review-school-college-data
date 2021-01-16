using System.Data.EntityClient;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {
        public static void GetAdjustmentRequestSummary()
        {

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {

                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {

                }
            }
        }
    }
}
