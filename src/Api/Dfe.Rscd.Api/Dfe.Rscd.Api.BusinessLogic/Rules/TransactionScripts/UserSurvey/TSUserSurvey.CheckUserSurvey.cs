using System;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSUserSurvey : Logic.TSBase
    {
        public static Boolean CheckUserSurvey(int DCSFNumber, String userName)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return context.UserSurvey.Any(us => us.Schools.DFESNumber == DCSFNumber && us.Username == userName);
                }
            }            
        }
    }
}
